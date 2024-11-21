
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TSMS_2_.DTO;
using TSMS_2_.Model;
using TSMS_2_.Services;
using TSMS_2_.EF;

namespace TSMS_2_.ViewModel
{
    public class SalesmanViewModel : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly SalesmanModel _salesmanModel = new SalesmanModel();
        private List<salesmanDTO> _salesmen;
        private readonly IWindowService _windowService;
        private salesmanDTO _selectedSalesman;
        public ICommand UpdObjInDBCommand { get; }
        public ICommand AddObjInDBCommand { get; }
        public ICommand AddObjCommand { get; }
        public ICommand DeleteObjCommand { get; }
        public ICommand RefreshObjCommand { get; }
        public ICommand EditObjCommand { get; }

        // Properties for Salesman details
        public string Number { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public long Salary { get; set; }
        public string Address { get; set; }
        public string Mail { get; set; }
       
        public SalesmanViewModel()
        {
            _salesmen = new List<salesmanDTO>();
            _windowService = new WindowService();
            AddObjInDBCommand = new RelayCommand(CreateSalesman);
            UpdObjInDBCommand = new RelayCommand(UpdateSalesman);
            AddObjCommand = new RelayCommand(OpenAddSalesman);
            EditObjCommand = new RelayCommand(OpenUpdSalesman);
            DeleteObjCommand = new RelayCommand(DeleteSelectedSalesman);
            RefreshObjCommand = new RelayCommand(RefreshSalesmen);
        }

      

        // List of salesmen
        public List<salesmanDTO> Salesmen
        {
            get
            {
                
                if (0 == _salesmen.Count)
                {
                    _salesmen = _tableModel.GetSalesmanDTO()/*.Select(i => new UserDTO(i)).ToList()*/;
                    //_users = /*new ObservableCollection<UserDTO>*/(us.Select(u => new UserDTO(u)));
                    OnPropertyChanged(nameof(Salesmen));
                }
                return _salesmen;
            }
            set
            {
                if (_salesmen != value)
                {
                    _salesmen = value;
                    OnPropertyChanged(nameof(Salesmen));
                }
            }
        }

        public salesmanDTO SelectedSalesman
        {
            get => _selectedSalesman;
            set
            {
                _selectedSalesman = value;
                OnPropertyChanged(nameof(SelectedSalesman));
            }
        }

        
        // Open the window to add a new salesman
        public void OpenAddSalesman()
        {
            SelectedSalesman = new salesmanDTO(); // Reset the selected salesman
            _windowService.OpenWindow("ADDSalesman", this, 1);
        }

        // Open the window to update an existing salesman
        public void OpenUpdSalesman()
        {
            if (SelectedSalesman != null)
            {
                SelectedSalesman = new salesmanDTO(SelectedSalesman); // Create a copy for editing
                _windowService.OpenWindow("ADDSalesman", this, 2);
            }
        }

        // Refresh the list of salesmen
        public void RefreshSalesmen()
        {
            var us = _tableModel.GetSalesmanDTO();
            Salesmen.Clear();
            Salesmen = _tableModel.GetSalesmanDTO()/*.Select(i => new UserDTO(i)).ToList()*/;
            OnPropertyChanged("Salesmen");
        }

        // Delete the selected salesman
        private void DeleteSelectedSalesman()
        {
            if (SelectedSalesman != null)
            {
                _salesmanModel.DeleteSalesman(SelectedSalesman.id);
                Salesmen.Remove(SelectedSalesman);
                RefreshSalesmen();
            }
        }

        // Update the selected salesman in the database
        private void UpdateSalesman()
        {
            if (SelectedSalesman != null)
            {
                _salesmanModel.UpdateSalesman(SelectedSalesman);
                RefreshSalesmen();
            }
        }

        // Create a new salesman in the database
        public void CreateSalesman()
        {
            _salesmanModel.CreateSalesman(SelectedSalesman);
            RefreshSalesmen();
        }

        // Property changed event handler
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
