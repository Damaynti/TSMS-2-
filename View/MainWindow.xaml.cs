using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TSMS_2_.Services;
using TSMS_2_.ViewModel;

namespace TSMS_2_.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            var windowService = new WindowService();
            DataContext = new MainWindowsVM(windowService);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную роль и пароль
            string selectedRole = RoleComboBox.SelectedItem is ComboBoxItem selectedItem ? selectedItem.Content.ToString() : "";
            string password = PasswordBox.Password;

            // Проверяем, что роль выбрана и пароль введен
            if (string.IsNullOrEmpty(selectedRole) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, выберите роль и введите пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Вызов метода входа из ViewModel
            var viewModel = (MainWindowsVM)DataContext;
            viewModel.Password = password; // Устанавливаем пароль в ViewModel
            viewModel.SelectedRole = selectedRole; // Устанавливаем роль в ViewModel

            // Выполняем вход
            viewModel.LoginCommand.Execute(null);
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                ((MainWindowsVM)DataContext).Password = passwordBox.Password;
            }
        }
    }
}
