using GraphManager.ViewModels;
using System.Windows;

namespace GraphManager;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 1. Создаем ViewModel
        var viewModel = new MainViewModel();

        // 2. Создаем View
        var view = new MainWindow();

        // 3. Связываем их!
        view.DataContext = viewModel;

        // 4. Показываем окно
        view.Show();
    }
}

