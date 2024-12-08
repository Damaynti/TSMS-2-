using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using TSMS_2_.DTO;
using TSMS_2_.EF;
using TSMS_2_.Model;
using TSMS_2_.Services;
using TSMS_2_.View;

namespace TSMS_2_.ViewModel
{
    public class SaleViewModel : INotifyPropertyChanged

    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly SalyModel _saleModel = new SalyModel();
        private List<SaleDTO> _sales;
        private readonly IWindowService _windowService;
        private SaleDTO _selectedSale;
        public ICommand AddSaleCommand { get; }
        public ICommand UpdateSaleCommand { get; }
        public ICommand DeleteSaleCommand { get; }
        public ICommand RefreshSalesCommand { get; }
        public ICommand EndCommand { get; }
        public ICommand UpdObjInDBCommand { get; }
        public ICommand AddObjInDBCommand { get; }
        public SaleViewModel()
        {
            _sales = new List<SaleDTO>();
            _windowService = new WindowService();
            AddObjInDBCommand = new RelayCommand(CreateSale);
            UpdObjInDBCommand = new RelayCommand(UpdateSelectedSale);
            AddSaleCommand = new RelayCommand(OpenAddSale);
            UpdateSaleCommand = new RelayCommand(OpenUpdateSale);
            DeleteSaleCommand = new RelayCommand(DeleteSelectedSale);
            RefreshSalesCommand = new RelayCommand(RefreshSales);
            EndCommand = new RelayCommand(End);
        }

        public void End()
        {
            var currentWindow = Application.Current.Windows.OfType<ADDSale>().FirstOrDefault();
            _windowService.CloseWindow(currentWindow);
        }
        public List<SaleDTO> Sales
        {
            get
            {
                if (_sales == null || _sales.Count == 0)
                {
                    RefreshSales();
                }
                return _sales;
            }
            set
            {
                if (_sales != value)
                {
                    _sales = value;
                    OnPropertyChanged(nameof(Sales));
                }
            }
        }
        private List<salesman> _salesman;
        public List<salesman> salesman
        {
            get
            {
                if (_salesman == null)
                {
                    _salesman = _tableModel.GetSalesmans();
                    OnPropertyChanged(nameof(salesman));
                }
                return _salesman;
            }
            set
            {
                if (_salesman != value)
                {
                    _salesman = value;
                    OnPropertyChanged(nameof(categories));
                }
            }
        }

        private List<client> _client;
        public List<client> client
        {
            get
            {
                if (_client == null)
                {
                    _client = _tableModel.GetClients();
                    OnPropertyChanged(nameof(client));
                }
                return _client;
            }
            set
            {
                if (_client != value)
                {
                    _client = value;
                    OnPropertyChanged(nameof(client));
                }
            }
        }
        public SaleDTO SelectedSale
        {
            get => _selectedSale;
            set
            {
                _selectedSale = value;
                OnPropertyChanged(nameof(SelectedSale));
            }
        }
        
        public void OpenAddSale()
        {
            SelectedSale = new SaleDTO();
            _windowService.OpenWindow("AddSale", this, 1);
        }
        public void OpenUpdateSale()
        {
            if (SelectedSale != null)
            {
                SelectedSale = new SaleDTO(SelectedSale);
                _windowService.OpenWindow("AddSale", this, 2);
            }
        }
        public void RefreshSales()
        {
            Sales = _tableModel.GetSaleDTO();
            OnPropertyChanged(nameof(Sales));
        }
        private void DeleteSelectedSale()
        {
            if (SelectedSale != null)
            {
                _saleModel.DeleteSale(SelectedSale.id);
                Sales.Remove(SelectedSale);
                RefreshSales();
            }
        }
        private void UpdateSelectedSale()
        {
            if (SelectedSale.salesmn_id != 0  && SelectedSale.client_id != 0)
            {
                _saleModel.UpdateSale(SelectedSale);
                RefreshSales();
                var currentWindow = Application.Current.Windows.OfType<ADDSale>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
            }
        }
        public void CreateSale()
        {
            if (SelectedSale.salesmn_id != 0  && SelectedSale.client_id!=0 )
            {
                _saleModel.CreateSale(SelectedSale);
                RefreshSales();
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