using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TSMS_2_.DTO;
using TSMS_2_.Model;
using TSMS_2_.Services;
using TSMS_2_.View;

namespace TSMS_2_.ViewModel
{
    public class SellerVM : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel;
        private List<ProductsDTO> _products;
        private ProductsDTO _selectedProduct;
        private ClientDTO _client;
        private readonly IWindowService _windowService;
        private string _searchNoom;
        private List<ClientDTO> _Clients;
        private ClientDTO _selectedClient;
        public ICommand RefreshClientsCommand { get; }
        public ICommand SearchClientsCommand { get; }
        public ICommand FindProductCommand { get; }
        public ICommand RefreshProductsCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand AddNewClientCommand { get; }
        public ICommand CleanCommand { get; }
        public ICommand OpenAddElementSaleCommand { get; }
        public ICommand Creat { get; }
        public ICommand OpenNoomberCommand { get; }
        public ICommand OpenADDCientCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand EndCommand { get; }
        public decimal TotalSum => CartItems.Sum(item => item.TotalPrice);
        public SellerVM(long idsal)
        {
            _tableModel = new TableModel();
            AddToCartCommand = new RelayCommand(AddToCart);
            OpenADDCientCommand = new RelayCommand(OpenADDClient);
            CleanCommand = new RelayCommand(Clean);
            OpenAddElementSaleCommand = new RelayCommand(OpenAddElementSale);
            Creat = new RelayCommand(CreateOrder);
            OpenNoomberCommand = new RelayCommand(OpenNoomber);
            _windowService = new WindowService();
            RemoveItemCommand = new Command<Element_saleDto>(RemoveItem);
            IncreaseQuantityCommand = new Command<Element_saleDto>(IncreaseQuantity);
            DecreaseQuantityCommand = new Command<Element_saleDto>(DecreaseQuantity);
            this.idsal = idsal;
            AddNewClientCommand = new RelayCommand(AddClient);
            _products = new List<ProductsDTO>();
            RefreshProductsCommand = new RelayCommand(RefreshProducts);
            FindProductCommand = new RelayCommand(FindProducts);
            _Clients = new List<ClientDTO>();
            RefreshClientsCommand = new RelayCommand(RefreshClients);
            SearchClientsCommand = new RelayCommand(AddToNoom);
            EndCommand = new RelayCommand(End);
            LoadProducts();
        }
        public void End()
        {
            _windowService.ShowWindow("MainWindow");
            var currentWindow = Application.Current.Windows.OfType<SellerWindow>().FirstOrDefault();
            _windowService.CloseWindow(currentWindow);
        }
        private readonly long idsal;
        private long? _idFilter;
        public long? IdFilter
        {
            get => _idFilter;
            set
            {
                _idFilter = value;
                OnPropertyChanged(nameof(IdFilter));
                FindProducts();
            }
        }
        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
                FindProducts();
            }
        }
        private string _phoneNumber = "Номер не указан";
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
        private string _newClientNumber;
        private readonly ClientModel _clientModel = new ClientModel();
        public string NewClientNumber
        {
            get => _newClientNumber;
            set
            {
                _newClientNumber = value;
                OnPropertyChanged();
            }
        }
        public string SearchNoom
        {
            get => _searchNoom;
            set
            {
                _searchNoom = value;
                OnPropertyChanged(nameof(SearchNoom));
                FindNoom();
            }
        }
        public ClientDTO SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
            }
        }
        public List<ClientDTO> Clients
        {
            get => _Clients;
            set
            {
                if (_Clients != value)
                {
                    _Clients = value;
                    OnPropertyChanged(nameof(Clients));
                }
            }
        }
        private List<ProductsDTO> _cart;
        public List<ProductsDTO> Cart
        {
            get => _cart ?? (_cart = new List<ProductsDTO>());
            set
            {
                _cart = value;
                OnPropertyChanged(nameof(Cart));
            }
        }
        public List<ProductsDTO> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ProductsDTO SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        private ObservableCollection<Element_saleDto> _cartItems;
        public ObservableCollection<Element_saleDto> CartItems
        {
            get => _cartItems ?? (_cartItems = new ObservableCollection<Element_saleDto>());
            set
            {
                if (_cartItems != null)
                    _cartItems.CollectionChanged -= CartItems_CollectionChanged;

                _cartItems = value;

                if (_cartItems != null)
                    _cartItems.CollectionChanged += CartItems_CollectionChanged;

                OnPropertyChanged(nameof(CartItems));
                OnPropertyChanged(nameof(TotalSum));
            }
        }
        private void CartItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(TotalSum));
        }
        public void AddToCart()
        {
            if (SelectedProduct != null)
            {
                var existingItem = CartItems.FirstOrDefault(item => item.ProductId == SelectedProduct.id);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    CartItems.Add(new Element_saleDto
                    {
                        ProductId = SelectedProduct.id,
                        ProductName = SelectedProduct.name,
                        ProductPrice = SelectedProduct.price,
                        Quantity = 1,
                    });
                }

                OnPropertyChanged(nameof(CartItems));
                OnPropertyChanged(nameof(TotalSum));

                var currentWindow = Application.Current.Windows.OfType<ADDElementSave>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
            }
        }
        public void RemoveItem(Element_saleDto item)
        {
            if (item != null)
            {
                CartItems.Remove(item);
                OnPropertyChanged(nameof(TotalSum));
            }
        }
        private void IncreaseQuantity(Element_saleDto item)
        {
            if (item != null)
            {
                item.Quantity++;
                OnPropertyChanged(nameof(TotalSum));
            }
        }
        private void DecreaseQuantity(Element_saleDto item)
        {
            if (item != null && item.Quantity > 1)
            {
                item.Quantity--;
                OnPropertyChanged(nameof(TotalSum));
            }
        }
        public void CreateOrder()
        {
            if (CartItems.Count == 0)
            {
                return;
            }

            var order = new SaleDTO()
            {
                cost = 0,
                salesmn_id = idsal,
                client_id = _client.id,
            };
            foreach (var item in CartItems)
            {
                order.cost += (long)item.TotalPrice;
            }
            var q = new SalyModel();
            var id = q.CreateSale(order);

            foreach (var item in CartItems)
            {
                var selectedElement = new Element_saleDto()
                {
                    SaleId = id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    price = (long)item.ProductPrice,
                };

                var elementSaleModel = new Element_saleModel();
                elementSaleModel.CreateElementSale(selectedElement);
            }
            CartItems.Clear();

            OnPropertyChanged(nameof(CartItems));

        }
        private void LoadProducts()
        {
            var productsFromDb = _tableModel.GetProductsDTO();
            Products = productsFromDb.ToList();
            OnPropertyChanged(nameof(Products));

            var clientsFromDb = _tableModel.GetClientDTO();
            Clients = clientsFromDb.ToList(); 
            OnPropertyChanged(nameof(Clients));
        }
        public void AddToNoom()
        {
            if (SelectedClient != null)
            {
                _client = SelectedClient;
                _phoneNumber = _client.noomber;
                OnPropertyChanged("PhoneNumber");
            }
            var currentWindow = Application.Current.Windows.OfType<Noomber>().FirstOrDefault();
            _windowService.CloseWindow(currentWindow);
        }
        public void Clean()
        {
            _phoneNumber = "Номер не указан";
            _cartItems = null;
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(PhoneNumber));
        }
        public void OpenNoomber()
        {
            _windowService.ShowWindow("Noomber", this);
        }
        public void OpenAddElementSale()
        {
            _windowService.ShowWindow("ADDElementSave", this);
        }
        public void OpenADDClient()
        {
            _windowService.ShowWindow("ADDClient", this);
        }
        public void RefreshProducts()
        {
            IdFilter = null;
            SearchTerm = null;
            LoadProducts();
        }
        private void FindProducts()
        {
            var allProducts = _tableModel.GetProductsDTO();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                allProducts = _tableModel.GetProductsDTOName(SearchTerm, allProducts);
            }

            if (IdFilter.HasValue)
            {
                allProducts = _tableModel.GetProductsDTOID(IdFilter.Value, allProducts);
            }

            Products = allProducts.ToList();
            OnPropertyChanged(nameof(Products));
        }
        public void AddClient()
        {
            var id = _clientModel.CreateClient(NewClientNumber);
            _client = _tableModel.GetClientDTOID(id);
            _phoneNumber = _client.noomber;
            var currentWindow = Application.Current.Windows.OfType<ADDClient>().FirstOrDefault();
            _windowService.CloseWindow(currentWindow);
            OnPropertyChanged("PhoneNumber");
            NewClientNumber=null;
            LoadProducts();
        }
        public void RefreshClients()
        {
            SearchNoom = null;
            LoadProducts();
        }
        private void FindNoom()
        {
            var allClient = _tableModel.GetClientDTO();

            if (!string.IsNullOrEmpty(SearchNoom))
            {
                allClient = _tableModel.GetnClientDTONoom(SearchNoom, allClient);
            }

            Clients = allClient.ToList();
            OnPropertyChanged(nameof(Clients));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}