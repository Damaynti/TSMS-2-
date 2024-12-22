using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TSMS_2_.Model;
using TSMS_2_.Services;
using TSMS_2_.EF;
using TSMS_2_.View;

namespace TSMS_2_.ViewModel
{
    public class AdminVM : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly IWindowService _windowService;

        public AdminVM()
        {
            _windowService = new WindowService();
            SaveChangesCommand = new RelayCommand(SaveChanges);
            DeleteSalesmanCommand = new RelayCommand(DeleteSelectedSalesman);
            EndCommand= new RelayCommand(End);
        }
        public ICommand AddSalesmanCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand DeleteSalesmanCommand { get; }
        public ICommand EndCommand { get; }

        private ObservableCollection<salesman> _salesmen;
        public ObservableCollection<salesman> Salesmen
        {
            get
            {
                if (_salesmen == null)
                {
                    _salesmen = new ObservableCollection<salesman>(_tableModel.GetSalesman());
                    OnPropertyChanged(nameof(Salesmen));
                }
                return _salesmen;
            }
        }

        public void End()
        {
            
            _windowService.ShowWindow("MainWindow");
            var currentWindow = Application.Current.Windows.OfType<AdminWindow>().FirstOrDefault();
            _windowService.CloseWindow(currentWindow);
        }

        private salesman _selectedSalesman;
        public salesman SelectedSalesman
        {
            get => _selectedSalesman;
            set
            {
                _selectedSalesman = value;
                OnPropertyChanged(nameof(SelectedSalesman));
            }
        }

        public void SaveChanges()
        {
            if (SelectedSalesman != null)
            {
                MessageBox.Show($"Сохранены изменения для: {SelectedSalesman.FullName}");
            }
        }

        private void DeleteSelectedSalesman()
        {
            if (SelectedSalesman != null)
            {
                Salesmen.Remove(SelectedSalesman);
                MessageBox.Show($"Удален продавец: {SelectedSalesman.FullName}");
                SelectedSalesman = null; 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Salesman
    {
        public string Name { get; set; }
        public string Position { get; set; }
    }
}

