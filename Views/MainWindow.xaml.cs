using GraphManager.Commands;
using GraphManager.Models;
using GraphManager.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace GraphManager
{
    public partial class MainWindow : Window
    {
        private bool _isDragging = false;
        private Point _clickOffset; // Смещение клика от угла блока
        private TaskBlock _draggedBlock; // Блок, который тащим
        private MainViewModel _viewModel;
        private Point _originalPosition;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                _viewModel = this.DataContext as MainViewModel;
            };
        }
        private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            var item = sender as MenuItem;
            if (item != null)
                item.IsSubmenuOpen = true;
        }
        private void MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            var item = sender as MenuItem;
            if (item != null)
                item.IsSubmenuOpen = false;
        }
        // 1. Начало перетаскивания (Клик по блоку)
        private void Block_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel == null) return;

            var border = sender as FrameworkElement;
            var clickedBlock = border.DataContext as TaskBlock;

            if (clickedBlock == null) return;

            if (e.ClickCount == 2)
            {
                var detailsWindow = new TaskDetailsWindow(clickedBlock);
                detailsWindow.ShowDialog();

                e.Handled = true;
                return;
            }

            _viewModel.HandleBlockClick(clickedBlock);

            if (_viewModel.CurrentTool == Enums.ToolMode.Delete || _viewModel.CurrentTool == Enums.ToolMode.Link)
            {
                e.Handled = true;
                return;
            }
            
            _draggedBlock = border.DataContext as TaskBlock;

            if (_viewModel.CurrentTool == Enums.ToolMode.Select)
            {
                _draggedBlock = clickedBlock;
                _isDragging = true;
                _clickOffset = e.GetPosition(border);
                _originalPosition = new Point(_draggedBlock.X, _draggedBlock.Y);

                border.CaptureMouse();
            }
            e.Handled = true;
        }

        // 2. Процесс перетаскивания (Движение мыши по Canvas/Grid)
        private void CanvasArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && _draggedBlock != null)
            {
                var canvas = sender as IInputElement;
                Point currentPosition = e.GetPosition(canvas);

                _draggedBlock.X = currentPosition.X - _clickOffset.X;
                _draggedBlock.Y = currentPosition.Y - _clickOffset.Y;
            }
        }

        // 3. Конец перетаскивания (Отпускаем кнопку)
        private void CanvasArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging && _draggedBlock != null)
            {
                if (!_viewModel.CurrentProject.Blocks.Contains(_draggedBlock))
                {
                    _isDragging = false;
                    _draggedBlock = null;
                    Mouse.Capture(null);
                    return;
                }

                // Пытаемся найти место
                bool success = _viewModel.ResolveCollision(_draggedBlock);

                // Если места нет — возвращаем на базу
                if (!success)
                {
                    _draggedBlock.X = _originalPosition.X;
                    _draggedBlock.Y = _originalPosition.Y;
                    
                    _viewModel.ShowNotification("В этом месте невозможно разместить блок!", 5);
                    
                }
                else
                {
                    if (Math.Abs(_draggedBlock.X - _originalPosition.X) > 1 || Math.Abs(_draggedBlock.Y - _originalPosition.Y) > 1)
                    {
                        var cmd = new MoveBlockCommand(
                            _draggedBlock,
                            _originalPosition.X,
                            _originalPosition.Y,
                            _draggedBlock.X,
                            _draggedBlock.Y
                        );
                        _viewModel.History.AddAndExecute( cmd );
                    }
                }
            }

            // Очистка
            if (_isDragging)
            {
                _isDragging = false;
                _draggedBlock = null;
                Mouse.Capture(null);
            }
        }

        // 4. Клик по пустому канвасу
        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel == null) return;
            if (_viewModel.CurrentTool == Enums.ToolMode.Create)
            {
                var position = e.GetPosition((IInputElement)sender);
                var newBlock = _viewModel.CreateBlockAt(position.X, position.Y);
                _viewModel.CurrentTool = Enums.ToolMode.Select;
                bool success = _viewModel.ResolveCollision(newBlock);
                if (!success)
                {
                    //_viewModel.DeleteBlock(newBlock);
                    _viewModel.CurrentProject.Blocks.Remove(newBlock);
                    _viewModel.History.DiscardLastUndo();
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        //MessageBox.Show("В этом месте невозможно создать блок!");
                        _viewModel.ShowNotification("В этом месте невозможно создать блок!", 5);
                    }), System.Windows.Threading.DispatcherPriority.Normal);
                }
            }
        }
    }
}