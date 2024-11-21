using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TSMS_2_.Model;
using TSMS_2_.Services;
using TSMS_2_.View;

namespace TSMS_2_.ViewModel
{
    public class MainWindowsVM : INotifyPropertyChanged
    {
        private IWindowService _windowService;
        private TableModel _tableModel;
        private string _password;
        private string _selectedRole;
        private bool _isAuthenticated;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoginCommand { get; }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            private set
            {
                _isAuthenticated = value;
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }

        public MainWindowsVM(IWindowService windowService)
        {
            _windowService = windowService;
            _tableModel = new TableModel();
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            bool r=true;
            if (SelectedRole=="Продавец")r=false;

            var us = _tableModel.ValidateUser(r,Password);
            if (us!=null)
            {
                IsAuthenticated = true;

                // Открытие нового окна в зависимости от роли
                string nextWindow = SelectedRole == "Продавец" ? "SellerWindow" : "AdminWindow";
                var nextViewModel = SelectedRole == "Продавец" ? (object)new SellerVM(us.id) : new AdminVM(_windowService);


                _windowService.ShowWindow(nextWindow,nextViewModel,us.id);

                // Закрытие текущего окна
                var currentWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
            }
            else
            {
                IsAuthenticated = false;
                MessageBox.Show("Неверный пароль или роль.");
            }
        }

        private bool ValidateCredentials(string role, string password)
        {
            // Здесь должна быть ваша логика проверки учетных данных.
            // Это может быть вызов к базе данных или другой источник данных.
            return !string.IsNullOrEmpty(role) && !string.IsNullOrEmpty(password) && password == "your_password"; // Пример проверки
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
