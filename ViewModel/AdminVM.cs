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

        }
        public ICommand AddSalesmanCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand DeleteSalesmanCommand { get; }

        public AdminVM(IWindowService windowService)
        {
            _windowService = windowService;
            //AddSalesmanCommand = new RelayCommand(AddSalesman);
            SaveChangesCommand = new RelayCommand(SaveChanges);
            DeleteSalesmanCommand = new RelayCommand(DeleteSelectedSalesman);
        }

        // ObservableCollection for Salesmen
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
            var mainWindow = new MainWindow();
            mainWindow.Show();
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

        // Method to add a new salesman
        //public void AddSalesman()
        //{
        //    var newSalesman = new salesman { FullName = "Новый Продавец", Position = "Продавец" };
        //    Salesmen.Add(newSalesman);
        //    SelectedSalesman = newSalesman; // Optionally select the newly added salesman
        //    _windowService.ShowWindow("AddSalesman"); // Show the add salesman window if needed
        //}

        // Method to save changes made to the selected salesman
        public void SaveChanges()
        {
            if (SelectedSalesman != null)
            {
                // Logic to save changes to the selected salesman
                // This could involve updating the database or another data source.
                MessageBox.Show($"Сохранены изменения для: {SelectedSalesman.FullName}");
                // Implement actual saving logic here
            }
        }

        // Method to delete the selected salesman
        private void DeleteSelectedSalesman()
        {
            if (SelectedSalesman != null)
            {
                Salesmen.Remove(SelectedSalesman);
                // Logic to delete from database or data source can be added here.
                MessageBox.Show($"Удален продавец: {SelectedSalesman.FullName}");
                SelectedSalesman = null; // Clear selection after deletion
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

