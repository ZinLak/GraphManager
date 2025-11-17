using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32; // Для Open/Save Dialog
using Newtonsoft.Json;
using GraphManager.Enums;
using GraphManager.Models;

namespace GraphManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        // 1. ЕДИНЫЙ ИСТОЧНИК ДАННЫХ

        private TaskProject _currentProject;
        public TaskProject CurrentProject
        {
            get => _currentProject;
            set { _currentProject = value; OnPropertyChanged(nameof(CurrentProject)); }
        }

        private ToolMode _currentTool;
        public ToolMode CurrentTool
        {
            get => _currentTool;
            set { _currentTool = value; OnPropertyChanged(nameof(CurrentTool)); }
        }

        private TaskBlock _linkSource; // Для создания связей

        // 2. КОМАНДЫ (Привязка к кнопкам в XAML)
        public ICommand SetCreateCmd => new RelayCommand(_ => CurrentTool = ToolMode.Create);
        public ICommand SetSelectCmd => new RelayCommand(_ => CurrentTool = ToolMode.Select);
        public ICommand SetDeleteCmd => new RelayCommand(_ => CurrentTool = ToolMode.Delete);
        public ICommand SetLinkCmd => new RelayCommand(_ => { CurrentTool = ToolMode.Link; _linkSource = null; });

        // ИСПРАВЛЕНО: Мы используем OpenProjectFile и SaveProjectFile,
        // потому что в них есть ВАЖНАЯ логика (починка связей).
        public ICommand LoadCmd { get; }
        public ICommand SaveCmd { get; }

        public MainViewModel()
        {
            CurrentProject = new TaskProject();
            CurrentTool = ToolMode.Select; // Задаем начальный режим

            // Привязываем команды к методам
            LoadCmd = new RelayCommand(OpenProjectFile);
            SaveCmd = new RelayCommand(SaveProjectFile);
        }

        // 3. ЛОГИКА (Методы, которые выполняют работу)

        public TaskBlock CreateBlockAt(double x, double y)
        {
            var b = new TaskBlock { X = x, Y = y };
            // ИСПРАВЛЕНО: Используем CurrentProject
            CurrentProject.Blocks.Add(b);
            return b;
        }

        public void DeleteBlock(TaskBlock block)
        {
            if (block == null) return;
            // ИСПРАВЛЕНО: Используем CurrentProject
            var linksToRemove = CurrentProject.Links.Where(l => l.SourceBlockId == block.Id || l.TargetBlockId == block.Id).ToList();
            foreach (var link in linksToRemove) CurrentProject.Links.Remove(link);
            CurrentProject.Blocks.Remove(block);
        }

        // Этот метод отличный! Он вызывается из code-behind (MainWindow.xaml.cs)
        public void HandleBlockClick(TaskBlock clicked)
        {
            switch (CurrentTool)
            {
                case ToolMode.Select:
                    // (Здесь можно открыть окно "Подробности")
                    break;
                case ToolMode.Create:
                    // (Клик по блоку в режиме "Создать" ничего не делает)
                    break;
                case ToolMode.Delete:
                    DeleteBlock(clicked);
                    break;
                case ToolMode.Link:
                    if (_linkSource == null)
                    {
                        _linkSource = clicked;
                    }
                    else
                    {
                        if (_linkSource != clicked)
                        {
                            // ИСПРАВЛЕНО: Используем CurrentProject
                            bool exists = CurrentProject.Links.Any(l => (l.SourceBlockId == _linkSource.Id && l.TargetBlockId == clicked.Id));
                            if (!exists)
                            {
                                // ВАЖНО: Тут мы создаем связь, но не назначаем
                                // SourceBlock/TargetBlock. Это нужно исправить.
                                var newLink = new TaskLink
                                {
                                    SourceBlockId = _linkSource.Id,
                                    TargetBlockId = clicked.Id,
                                    SourceBlock = _linkSource,      // <--- ДОБАВЛЕНО
                                    TargetBlock = clicked           // <--- ДОБАВЛЕНО
                                };
                                CurrentProject.Links.Add(newLink);
                            }
                        }
                        _linkSource = null;
                        CurrentTool = ToolMode.Select; // Сбрасываем режим
                    }
                    break;
            }
        }

        public void DeleteLink(TaskLink link) => CurrentProject.Links.Remove(link);

        // ВАЖНО: Этот метод требует, чтобы у TaskBlock были свойства Width и Height. Убедитесь, что они есть.
        public bool IsOverlapping(TaskBlock blockToCheck)
        {
            // Убедитесь, что у TaskBlock есть Width и Height
            var rect1 = new System.Windows.Rect(blockToCheck.X, blockToCheck.Y, 150, 80); // <--- Замените на реальные Width/Height

            // ИСПРАВЛЕНО: Используем CurrentProject
            foreach (var other in CurrentProject.Blocks)
            {
                if (other.Id == blockToCheck.Id) continue;
                var rect2 = new System.Windows.Rect(other.X, other.Y, 150, 80); // <--- Замените на реальные Width/Height
                if (rect1.IntersectsWith(rect2)) return true;
            }
            return false;
        }

        // 4. ЛОГИКА ФАЙЛОВ (Сохранение и Загрузка)

        private void SaveProjectFile(object obj)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Project Files (*.json)|*.json";
            if (saveDialog.ShowDialog() == true)
            {
                string json = JsonConvert.SerializeObject(CurrentProject, Formatting.Indented);
                File.WriteAllText(saveDialog.FileName, json);
            }
        }

        private void OpenProjectFile(object obj)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Project Files (*.json)|*.json";
            if (openDialog.ShowDialog() == true)
            {
                string json = File.ReadAllText(openDialog.FileName);
                var loadedProject = JsonConvert.DeserializeObject<TaskProject>(json);

                // КРИТИЧЕСКИ ВАЖНЫЙ КОД Восстановление связей
                if (loadedProject.Links != null)
                {
                    foreach (var link in loadedProject.Links)
                    {
                        link.SourceBlock = loadedProject.Blocks.FirstOrDefault(b => b.Id == link.SourceBlockId);
                        link.TargetBlock = loadedProject.Blocks.FirstOrDefault(b => b.Id == link.TargetBlockId);
                    }
                }

                CurrentProject = loadedProject;
            }
        }
    }
}