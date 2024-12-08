using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private ComboBoxItem _selectedRole;
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
        public ComboBoxItem SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (_selectedRole != value)
                {
                    _selectedRole = value;
                    OnPropertyChanged(nameof(SelectedRole));
                }
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
        public MainWindowsVM()
        {
            _windowService = new WindowService();
            _tableModel = new TableModel();
            LoginCommand = new RelayCommand(Login);
        }
        private void Login()
        {
            bool r=true;
            if ((string)SelectedRole.Content== "Продавец")
                r=false;
            var us = _tableModel.ValidateUser(r,Password);
            if (us!=null)
            {
                IsAuthenticated = true;
                string nextWindow = (string)SelectedRole.Content == "Продавец" ? "SellerWindow" : "AdminWindow";
                var nextViewModel = (string)SelectedRole.Content == "Продавец" ? (object)new SellerVM(us.id) : new AdminVM();
                _windowService.ShowWindow(nextWindow,nextViewModel);
                var currentWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
            }
            else
            {
                IsAuthenticated = false;
                MessageBox.Show("Неверный пароль или роль.");
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
