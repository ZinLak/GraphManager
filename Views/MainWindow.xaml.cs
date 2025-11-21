using GraphManager.ViewModels;
using GraphManager.Models;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace GraphManager
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

        // 1. Начало перетаскивания (Клик по блоку)
        private void Block_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel == null) return;

            var border = sender as FrameworkElement;
            var clickedBlock = border.DataContext as TaskBlock;

            if (clickedBlock == null) return;

            _viewModel.HandleBlockClick(clickedBlock);

            _draggedBlock = border.DataContext as TaskBlock; // Получаем объект данных

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
                // Получаем текущие координаты мыши относительно основного контейнера
                var canvas = sender as IInputElement;
                Point currentPosition = e.GetPosition(canvas);

                // Обновляем координаты в ViewModel (TaskBlock)
                // Благодаря TwoWay Binding в XAML, блок на экране тоже сдвинется!
                _draggedBlock.X = currentPosition.X - _clickOffset.X;
                _draggedBlock.Y = currentPosition.Y - _clickOffset.Y;

                // Позже можно добавить проверку на пересечение _viewModel.IsOverlapping (ГОТОВО)
            }
        }

        // 3. Конец перетаскивания (Отпускаем кнопку)
        private void CanvasArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging && _draggedBlock != null)
            {
                bool isOverlapped = _viewModel.IsOverlapping(_draggedBlock);
                if (isOverlapped)
                {
                    _draggedBlock.X = _originalPosition.X;
                    _draggedBlock.Y = _originalPosition.Y;
                    // Можно будет добавить звук ошибки или массаж бокс.
                }
                _isDragging = false;
                _draggedBlock = null;
                Mouse.Capture(null);
            }
            if (_isDragging) 
            {
                // условие для подстраховки (оно не обязательно,
                // но если вдруг draggedBlock стал null, а флаг перетаскивания true (маловероятно))
                _isDragging = false;
                _draggedBlock = null;
                Mouse.Capture(null);
            }
        }

        // 4. Клик по пустому канвасу)
        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel == null) return;
            if (_viewModel.CurrentTool == Enums.ToolMode.Create)
            {
                var position = e.GetPosition((IInputElement)sender);
                _viewModel.CreateBlockAt(position.X, position.Y);
                // Сбрасываем режим, чтобы не создавать блоки по новому клику
                _viewModel.CurrentTool = Enums.ToolMode.Select;
            }
        }
    }
}