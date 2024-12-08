
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
using TSMS_2_.View;
using System.Windows;

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
        public ICommand EndCommand { get; }
       
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
            EndCommand = new RelayCommand(End);
        }
        public void End()
        {
            var currentWindow = Application.Current.Windows.OfType<ADDSalesmenxamlxaml>().FirstOrDefault();
            _windowService.CloseWindow(currentWindow);
        }
        public List<salesmanDTO> Salesmen
        {
            get
            {
                if (0 == _salesmen.Count)
                {
                    _salesmen = _tableModel.GetSalesmanDTO();
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
        public void OpenAddSalesman()
        {

            SelectedSalesman = new salesmanDTO();
            _windowService.OpenWindow("ADDSalesman", this, 1);

        }

        public void OpenUpdSalesman()
        {
            if (SelectedSalesman!=null&&SelectedSalesman.id!=0)
            {
                SelectedSalesman = new salesmanDTO(SelectedSalesman); 
                _windowService.OpenWindow("ADDSalesman", this, 2);
            }
        }
        public void RefreshSalesmen()
        {
            var us = _tableModel.GetSalesmanDTO();
            Salesmen.Clear();
            Salesmen = _tableModel.GetSalesmanDTO();
            OnPropertyChanged("Salesmen");
        }

        private void DeleteSelectedSalesman()
        {
            if (SelectedSalesman != null)
            {
                _salesmanModel.DeleteSalesman(SelectedSalesman.id);
                Salesmen.Remove(SelectedSalesman);
                RefreshSalesmen();
            }
        }
        private void UpdateSalesman()
        {
            if (SelectedSalesman.FullName != null && SelectedSalesman.mail != null && SelectedSalesman.password != null && SelectedSalesman.number != null && SelectedSalesman.salary != 0 && SelectedSalesman.address != null)
            {
                _salesmanModel.UpdateSalesman(SelectedSalesman);
                RefreshSalesmen();
                End();
            }
        }
        public void CreateSalesman()
        {
            if (SelectedSalesman.FullName != null && SelectedSalesman.mail != null && SelectedSalesman.password != null && SelectedSalesman.number != null && SelectedSalesman.salary != 0 && SelectedSalesman.address != null)
            {
                _salesmanModel.CreateSalesman(SelectedSalesman);
                RefreshSalesmen();
                End();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
