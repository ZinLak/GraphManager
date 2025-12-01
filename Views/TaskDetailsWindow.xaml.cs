using GraphManager.Models;
using System.Windows;

namespace GraphManager
{
    public partial class TaskDetailsWindow : Window
    {
        public TaskDetailsWindow(TaskBlock block)
        {
            InitializeComponent();
            // Устанавливаем блок как DataContext. 
            // Это позволяет окну напрямую редактировать свойства блока.
            this.DataContext = block;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}