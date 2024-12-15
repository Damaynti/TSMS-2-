using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TSMS_2_.DTO;
using TSMS_2_.EF;
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
        public long _discount = 0;
        public decimal TotalSum
        {
            get
            {
                var total = CartItems.Sum(item => item.TotalPrice); // Сумма всех товаров
                var discountFactor = 100 - _discount;         // Коэффициент скидки
                return total * discountFactor/100;
            }
        }

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
        private string _newClientName;
        public string NewClientName
        {
            get => _newClientName;
            set
            {
                _newClientName = value;
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
                if (_selectedClient != value)
                {
                    _selectedClient = value;
                    OnPropertyChanged(nameof(SelectedClient));
                }
            }
        }
        private ComboBoxItem _selectedTClient;
        public ComboBoxItem SelectedTClient
        {
            get => _selectedTClient;
            set
            {
                if (_selectedTClient != value)
                {
                    _selectedTClient = value;
                    OnPropertyChanged(nameof(SelectedTClient));
                }
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
                var existingItem = CartItems.FirstOrDefault(item => item.products_id == SelectedProduct.id);

                if (existingItem != null)
                {
                    if (existingItem.Quantity < SelectedProduct.count)
                    {
                        existingItem.Quantity++;
                        OnPropertyChanged(nameof(TotalSum));
                    }
                    else
                    {
                        MessageBox.Show("Недостаточно товара на складе.");
                    }
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
                // Найти продукт, соответствующий текущему элементу корзины
                var product = Products.FirstOrDefault(p => p.id == item.products_id);

                if (product != null && item.Quantity < product.count)
                {
                    item.Quantity++;
                    OnPropertyChanged(nameof(TotalSum));
                }
                else
                {
                    MessageBox.Show("Невозможно увеличить количество. Товар недоступен в большем количестве.");
                }
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
                MessageBox.Show("Корзина пуста. Невозможно оформить заказ.");
                return;
            }

            var q = new SalyModel();
            var id = q.CreatOrder(_client, (long)TotalSum, idsal,  CartItems);
            SaveReceiptAsPdf(id);

            CartItems.Clear();
            _phoneNumber = "Номер не указан";
            On();
        }
        public void On()
        {
            OnPropertyChanged(nameof(PhoneNumber));
            OnPropertyChanged(nameof(TotalSum));
            OnPropertyChanged(nameof(CartItems));
        }
        public void SaveReceiptAsPdf(long orderId)
        {
            // Подтвердить, хочет ли пользователь сохранить чек
            var result = MessageBox.Show("Хотите ли вы сохранить чек?", "Сохранение чека", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            // Выбор пути сохранения
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"Чек_{orderId}",
                DefaultExt = ".pdf",
                Filter = "PDF файлы (*.pdf)|*.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    // Создание PDF документа
                    using (var document = new PdfDocument())
                    {
                        document.Info.Title = "Чек заказа";

                        // Добавление страницы
                        var page = document.AddPage();
                        using (var gfx = XGraphics.FromPdfPage(page))
                        {
                            // Настройка шрифта
                            var font = new XFont("Verdana", 12, XFontStyle.Regular);
                            var boldFont = new XFont("Verdana", 12, XFontStyle.Bold);

                            double yPoint = 40;
                            gfx.DrawString("Чек заказа", boldFont, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopCenter);
                            yPoint += 30;

                            // Информация о заказе
                            gfx.DrawString($"Номер покупки: {orderId}", font, XBrushes.Black, new XRect(40, yPoint, page.Width - 80, page.Height), XStringFormats.TopLeft);
                            yPoint += 20;

                            // Добавление даты и времени покупки
                            string purchaseDateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                            gfx.DrawString($"Дата и время покупки: {purchaseDateTime}", font, XBrushes.Black, new XRect(40, yPoint, page.Width - 80, page.Height), XStringFormats.TopLeft);
                            yPoint += 20;

                            gfx.DrawString("Товары:", boldFont, XBrushes.Black, new XRect(40, yPoint, page.Width - 80, page.Height), XStringFormats.TopLeft);
                            yPoint += 20;

                            // Перебор товаров в заказе
                            foreach (var item in CartItems)
                            {
                                gfx.DrawString($"{item.ProductName} - {item.Quantity} x {item.ProductPrice} = {item.TotalPrice}", font, XBrushes.Black, new XRect(60, yPoint, page.Width - 100, page.Height), XStringFormats.TopLeft);
                                yPoint += 20;
                            }

                            // Итоговая информация
                            yPoint += 20;
                            gfx.DrawString($"Итоговая сумма: {TotalSum}", boldFont, XBrushes.Black, new XRect(40, yPoint, page.Width - 80, page.Height), XStringFormats.TopLeft);

                            if (_discount > 0)
                            {
                                yPoint += 20;
                                gfx.DrawString($"Применена скидка: {_discount}%", font, XBrushes.Black, new XRect(40, yPoint, page.Width - 80, page.Height), XStringFormats.TopLeft);
                            }
                        }

                        // Сохранение PDF
                        document.Save(filePath);
                    }

                    MessageBox.Show("Чек успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при сохранении чека: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadProducts()
        {
            var productsFromDb = _tableModel.GetProductsDTO();

            // Filter out products with quantity 0
            var availableProducts = productsFromDb.Where(p => p.count > 0).ToList();

            Products = availableProducts;
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
                _discount = (long)_client._discount;
                OnPropertyChanged("PhoneNumber");
            }
            OnPropertyChanged(nameof(TotalSum));
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
            SelectedClient = new ClientDTO { physical_person = true };
            _windowService.OpenWindow("ADDClient", this,1);
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
            Products = allProducts.Where(p => p.count > 0).ToList();
            OnPropertyChanged(nameof(Products));
        }
          private void AddClient()
        {
            if (SelectedClient != null && !string.IsNullOrWhiteSpace(SelectedClient.noomber))
            {
                // Проверка корректности номера телефона
                if (!Regex.IsMatch(SelectedClient.noomber, @"^(\+7|8)\d{10}$"))
                {
                    MessageBox.Show(
                        "Номер телефона некорректен. Убедитесь, что он содержит только цифры и соответствует российскому формату (например, +79123456789 или 89123456789).",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Проверка, существует ли номер в базе данных
                if (_tableModel.DoesClientNumberExist(SelectedClient.noomber))
                {
                    MessageBox.Show(
                        "Клиент с таким номером телефона уже существует в базе данных.",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }
                if (SelectedClient.tClient != "Физическое лицо")
                {
                    SelectedClient.physical_person = false;
                }
                else SelectedClient.physical_person = true;
                if (SelectedClient.name == null) SelectedClient.name = "неизвестный";
                var idD = _tableModel.FindDiscountIdByPurchaseAmount(SelectedClient.purchase_amount);
                if (idD != null)
                {
                    SelectedClient.discount_id = (long)idD;
                }
                // Добавление клиента в базу
                SelectedClient.id = _clientModel.CreateClient(SelectedClient);
                PhoneNumber=SelectedClient.noomber;
                MessageBox.Show(
                    "Клиент успешно добавлен.",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                LoadProducts();
                var currentWindow = Application.Current.Windows.OfType<ADDClient>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
            }
            else
            {
                MessageBox.Show(
                    "Не заполнены обязательные поля (номер телефона).",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
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