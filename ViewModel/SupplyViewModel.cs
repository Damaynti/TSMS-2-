using PdfSharp.Pdf.Content.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TSMS_2_.DTO;
using TSMS_2_.EF;
using TSMS_2_.Model;
using TSMS_2_.Services;
using TSMS_2_.View;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TSMS_2_.ViewModel
{
    public class SupplyViewModel : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly SupplyModel _supplyModel = new SupplyModel();
        private readonly loanAgreementModel _loanAgreementModel = new loanAgreementModel();
        private ObservableCollection<SupplyDTO> _supplies;
        private List<ProductsDTO> _products;
        private List<SupplierDTO> _sup;
        private readonly ProductsModel _productsModel = new ProductsModel();
        private ProductsDTO _selectedProduct;
        private readonly IWindowService _windowService;
        private SupplyDTO _selectedSupply;
        public ICommand UpdateSupplierCommand { get; }
        public ICommand ADDProductsCommand { get; }
        public ICommand CleanCommand { get; }
        public ICommand OpenLoanAgreenentCommand { get; }
        public ICommand OpenSupCommand { get; }
        public ICommand RefreshProductsCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand AddSupplyCommand { get; }
        public ICommand UpdateSupplyCommand { get; }
        public ICommand DeleteSupplyCommand { get; }
        public ICommand RefreshSuppliesCommand { get; }
        public ICommand OpenAddElementSaleCommand { get; }
        public ICommand FindProductCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand AddObjInDBCommand { get; }
        public ICommand NullLoanAgreementCommand { get; }   
        public ICommand CreateCommand { get; }
        public ICommand AddObjCommand { get; }

        // Properties for Supply details
        public long SupplierId { get; set; }
        public DateTime Data { get; set; }
        public long Cost { get; set; }
        public decimal TotalSum
        {
            get
            {
                var total = CartItems.Sum(item => item.TotalPrice); 
                return (decimal)( (double)total * 0.7);
            }
        }

        public SupplyViewModel()
        {
            _supplies = new ObservableCollection<SupplyDTO>();
            _windowService = new WindowService();
            RemoveItemCommand = new Command<Element_saleDto>(RemoveItem);
            AddSupplyCommand = new RelayCommand(OpenAddSupply);
            OpenSupCommand=new RelayCommand(OpenAddSup);
            ADDProductsCommand = new RelayCommand(OpenAddProduct);
            AddObjCommand = new RelayCommand(CreateProduct);
            AddObjInDBCommand = new RelayCommand(CreateLoanAgreement);
            OpenLoanAgreenentCommand = new RelayCommand(OpenLoanAgreenent);
            UpdateSupplyCommand = new RelayCommand(OpenUpdateSupply, CanExecuteSelectedSupply);
            DeleteSupplyCommand = new RelayCommand(DeleteSelectedSupply, CanExecuteSelectedSupply);
            RefreshSuppliesCommand = new RelayCommand(LoadSupplies);
            OpenAddElementSaleCommand = new RelayCommand(OpenAddElementSale);
            RefreshProductsCommand = new RelayCommand(RefreshProducts);
            FindProductCommand = new RelayCommand(FindProducts);
            AddToCartCommand = new RelayCommand(AddToCart);
            CleanCommand = new RelayCommand(Clean);
            UpdateSupplierCommand = new RelayCommand(AddToSup);
            NullLoanAgreementCommand=new RelayCommand(NullLoanAgreement);
            _products = new List<ProductsDTO>();
            CreateCommand= new RelayCommand(CreateOrder);
            _sup = new List<SupplierDTO>();
            IncreaseQuantityCommand = new Command<Element_saleDto>(IncreaseQuantity);
            DecreaseQuantityCommand = new Command<Element_saleDto>(DecreaseQuantity);
            LoadSupplies();
        }
        public void CreateProduct()
        {
            if (SelectedProduct.name != null && SelectedProduct.categoris_id!= null && SelectedProduct.price != 0 && SelectedProduct.count != null)
            {
                if (SelectedProduct.tex == null)
                {
                    SelectedProduct.tex = "нет данных";
                }
                _productsModel.CreateProduct(SelectedProduct);
                LoadSupplies();
                var currentWindow = Application.Current.Windows.OfType<ADDProduct>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
                AddToCart();
            }
        }
        public void OpenAddProduct()
        {
            SelectedProduct = new ProductsDTO();
            _windowService.OpenWindow("AddProduct", this, 1);
        }
        private void NullLoanAgreement()
        {
            SelectedLoanAgreement=null;
            var currentWindow = Application.Current.Windows.OfType<ADDLoanAgreement>().FirstOrDefault();
            _windowService.CloseWindow(currentWindow);
        }
        private List<categories> _categories;

        public List<categories> categories
        {
            get
            {
                if (_categories == null)
                {
                    _categories = _tableModel.GetCategories();
                    OnPropertyChanged(nameof(categories));
                }
                return _categories;
            }
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged(nameof(categories));
                }
            }
        }

        public void CreateOrder()
        {
            if (CartItems.Count == 0)
            {
                MessageBox.Show("Корзина пуста. Невозможно оформить поставку.");
                return;
            }
            if (idsal == 0)
            {
                MessageBox.Show("Поставщик не выбран. Невозможно оформить поставку.");
                return;
            }
            var q = new SupplyModel();
            var id = q.CreatOrder(SelectedLoanAgreement, (long)TotalSum, idsal, CartItems);
            //SaveReceiptAsPdf(id);

            CartItems.Clear();
            _LoanAg = "Нет";
            _nameSup = "Поставщик не указан";
            LoadSupplies() ;    
        }


        private void CreateLoanAgreement()
        {
            if (SelectedLoanAgreement != null && SelectedLoanAgreement.end!= null && SelectedLoanAgreement.end>DateTime.Now )
            {
                _LoanAg = "Да";
                var currentWindow = Application.Current.Windows.OfType<ADDLoanAgreement>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
                LoadSupplies() ;
            }
            
        }
        
        private string _nameSup = "Поставщик не указан";
        public string NameSup
        {
            get => _nameSup;
            set
            {
                _nameSup = value;
                OnPropertyChanged(nameof(NameSup));
            }
        }
        private string _LoanAg = "Нет";
        public string LoanAg
        {
            get => _LoanAg;
            set
            {
                _LoanAg = value;
                OnPropertyChanged(nameof(LoanAg));
            }
        }
        public void AddToSup()
        {
            if (SelectedSup != null)
            {
                _selectedSup = SelectedSup;
                _nameSup = _selectedSup.FullName;
                idsal = _selectedSup.id;
                OnPropertyChanged("NameSup");
            }
            OnPropertyChanged(nameof(TotalSum));
            var currentWindow = Application.Current.Windows.OfType<SupAddSup>().FirstOrDefault();
            _windowService.CloseWindow(currentWindow);

        }
        public void RemoveItem(Element_saleDto item)
        {
            if (item != null)
            {
                CartItems.Remove(item);
                OnPropertyChanged(nameof(TotalSum));
            }
        }
        public void Clean()
        {
            _nameSup = "Поставщик не указан";
            _LoanAg = "Нет";
            _cartItems = null;
            LoadSupplies();
            OnPropertyChanged(nameof(CartItems));
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
                var existingItem = CartItems.FirstOrDefault(item => item.products_id == SelectedProduct.id);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                    OnPropertyChanged(nameof(TotalSum));
                }
                else
                {
                    CartItems.Add(new Element_saleDto
                    {
                        products_id = SelectedProduct.id,
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
        public void RefreshProducts()
        {
            IdFilter = null;
            SearchTerm = null;
            LoadSupplies();
        }
        public void OpenAddElementSale()
        {
            
            _windowService.OpenWindow("ADDElementSave", this, 2);
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
        // List of supplies
        public ObservableCollection<SupplyDTO> Supplies
        {
            get => _supplies;
            set
            {
                if (_supplies != value)
                {
                    _supplies = value;
                    OnPropertyChanged();
                }
            }
        }
        private  long idsal;
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
            Products = allProducts.Where(p => p.count > 0).ToList();
            OnPropertyChanged(nameof(Products));
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
        public SupplyDTO SelectedSupply
        {
            get => _selectedSupply;
            set
            {
                _selectedSupply = value;
                OnPropertyChanged();
                UpdateCommandStates();
            }
        }

        // Load supplies from the database
        private void LoadSupplies()
        {
            try
            {

                var supplyList = _tableModel.GetSupplyDTO(); // Получаем список поставок из модели
                Supplies = new ObservableCollection<SupplyDTO>(supplyList);
                var productsFromDb = _tableModel.GetProductsDTO();
                var clientsFromDb = _tableModel.GetClientDTO();
                var supList = _tableModel.GetSupplierDTO();
                Sup = supList.ToList();
                OnPropertyChanged(nameof(Sup));
                var availableProducts = productsFromDb.ToList();
                OnPropertyChanged(nameof(LoanAg));

                Products = availableProducts;
                OnPropertyChanged(nameof(Products));
            }
            catch (Exception ex)
            {
                // Обработка ошибок загрузки
                Console.WriteLine($"Error loading supplies: {ex.Message}");
            }
        }
        
        // Open the window to add a new supply
        public void OpenAddSupply()
        {
            var currentWindow = Application.Current.Windows.OfType<AdminWindow>().FirstOrDefault();
            _windowService.HideWindow(currentWindow);
            SelectedSupply = new SupplyDTO(); // Сбрасываем выбранную поставку
            _windowService.OpenWindow("ADDSupply", this, 1);
            LoadSupplies();
        }

        public void Show()
        {
            var currentWindow = Application.Current.Windows.OfType<AdminWindow>().FirstOrDefault();
            _windowService.SWindow(currentWindow);
        }
        private SupplierDTO _selectedSup;
        public SupplierDTO SelectedSup
        {
            get => _selectedSup;
            set
            {
                if (_selectedSup != value)
                {
                    _selectedSup = value;
                    OnPropertyChanged(nameof(SelectedSup));
                }
            }
        }
        private loanAgreementDTO _selectedLoanAgreement;
        public loanAgreementDTO SelectedLoanAgreement
        {
            get => _selectedLoanAgreement;
            set
            {
                if (_selectedLoanAgreement != value)
                {
                    _selectedLoanAgreement = value;
                    OnPropertyChanged(nameof(SelectedLoanAgreement));
                }
            }
        }
        public List<SupplierDTO> Sup
        {
            get => _sup;
            set
            {
                if (_sup != value)
                {
                    _sup = value;
                    OnPropertyChanged(nameof(Sup));
                }
            }
        }

        public void OpenLoanAgreenent()
        {
            if (SelectedLoanAgreement == null)
            {
                SelectedLoanAgreement = new loanAgreementDTO();
                _windowService.OpenWindow("ADDLoanAgreement", this, 1);
            }
            else
            {
                SelectedLoanAgreement = new loanAgreementDTO(SelectedLoanAgreement); // Create a copy for editing
                _windowService.OpenWindow("ADDLoanAgreement", this, 2);
            }
        }
        public void OpenAddSup()
        {
            SelectedSup = new SupplierDTO(); // Сбрасываем выбранную поставку
            _windowService.ShowWindow("ADDSup", this);
        }
        // Open the window to update an existing supply
        public void OpenUpdateSupply()
        {
            if (SelectedSupply != null)
            {
                var supplyToUpdate = new SupplyDTO(SelectedSupply); // Создаем копию для редактирования
                _windowService.OpenWindow("ADDSupply", supplyToUpdate, 2);
                LoadSupplies();
            }
        }

        // Refresh the list of supplies
      
        // Delete the selected supply
        private void DeleteSelectedSupply()
        {
            if (SelectedSupply != null)
            {
                try
                {
                    _supplyModel.DeleteSupply(SelectedSupply.id);
                    Supplies.Remove(SelectedSupply);
                    LoadSupplies();
                }
                catch (Exception ex)
                {
                    // Обработка ошибок удаления
                    Console.WriteLine($"Error deleting supply: {ex.Message}");
                }
            }
        }

        // Update the selected supply in the database
        private void UpdateSelectedSupply()
        {
            if (SelectedSupply != null)
            {
                try
                {
                    _supplyModel.UpdateSupply(SelectedSupply);
                    LoadSupplies();
                }
                catch (Exception ex)
                {
                    // Обработка ошибок обновления
                    Console.WriteLine($"Error updating supply: {ex.Message}");
                }
            }
        }

        // Update the state of commands that depend on SelectedSupply
        private void UpdateCommandStates()
        {
            (UpdateSupplyCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeleteSupplyCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        // Check if a supply is selected
        private bool CanExecuteSelectedSupply()
        {
            return SelectedSupply != null;
        }

        // Property changed event handler
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // RelayCommand implementation
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => _execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
