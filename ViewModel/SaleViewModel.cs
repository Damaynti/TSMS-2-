using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TSMS_2_.DTO;
using TSMS_2_.EF;
using TSMS_2_.Model;
using TSMS_2_.Services;

namespace TSMS_2_.ViewModel { 
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

    // Properties for Sale details
    public TimeSpan? Data { get; set; } // Время продажи
    public long Cost { get; set; } // Стоимость продажи
    public long SalesmanId { get; set; } // ID продавца
    public long ClientId { get; set; } // ID клиента
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
    }

    // List of sales
    public List<SaleDTO> Sales
    {
        get
        {
            if (_sales == null || _sales.Count == 0)
            {
                LoadSales();
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

    // Load sales from the database
    private void LoadSales()
    {
        Sales = _tableModel.GetSaleDTO(); // Получаем список продаж из модели
        OnPropertyChanged(nameof(Sales));
    }

    // Open the window to add a new sale
    public void OpenAddSale()
    {
        SelectedSale = new SaleDTO(); // Сбрасываем выбранную продажу
        _windowService.OpenWindow("AddSale", this, 1);
    }

    // Open the window to update an existing sale
    public void OpenUpdateSale()
    {
        if (SelectedSale != null)
        {
            SelectedSale = new SaleDTO(SelectedSale); // Создаем копию для редактирования
            _windowService.OpenWindow("AddSale", this, 2);
        }
    }

    // Refresh the list of sales
    public void RefreshSales()
    {
        LoadSales();
        OnPropertyChanged(nameof(Sales));
    }

    // Delete the selected sale
    private void DeleteSelectedSale()
    {
        if (SelectedSale != null)
        {
            _saleModel.DeleteSale(SelectedSale.id);
            Sales.Remove(SelectedSale);
            RefreshSales();
        }
    }

    // Update the selected sale in the database
    private void UpdateSelectedSale()
    {
        if (SelectedSale != null)
        {
            _saleModel.UpdateSale(SelectedSale);
            RefreshSales();
        }
    }

    // Create a new sale in the database
    public void CreateSale()
    {
        if (SelectedSale != null)
        {
            _saleModel.CreateSale(SelectedSale);
            RefreshSales();
        }
    }

    // Property changed event handler
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
}