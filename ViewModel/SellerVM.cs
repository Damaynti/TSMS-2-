using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TSMS_2_.DTO; // Убедитесь, что у вас есть этот класс
using TSMS_2_.EF;
using TSMS_2_.Model; // Модель для работы с продуктами
using TSMS_2_.Services; // Сервисы для работы с данными
using TSMS_2_.View; // Для доступа к окну ADDElementSale

namespace TSMS_2_.ViewModel
{
    public class SellerVM : INotifyPropertyChanged
    {
        //private readonly ProductsModel _productsModel; // Модель для работы с продуктами
        private readonly TableModel _tableModel;
        private List<ProductsDTO> _products; // Список доступных продуктов
        private ProductsDTO _selectedProduct; // Выбранный продукт
        private ClientDTO _client;
        private readonly IWindowService _windowService;

        public ICommand FindProductCommand { get; }
        public ICommand RefreshProductsCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand AddNewClientCommand {  get; }

        private readonly long idsal;
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
        private ObservableCollection<Element_saleDto> _cartItems;

        // Инициализация с пустой корзиной
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
                OnPropertyChanged(nameof(TotalSum));  // Обновить сумму при изменении корзины
            }
        }

        // Обновляем сумму при изменении коллекции
        private void CartItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(TotalSum)); // Обновляем TotalSum после изменения корзины
        }

        // Общая сумма товаров в корзине
        public decimal TotalSum => CartItems.Sum(item => item.TotalPrice);

        // Метод добавления товара в корзину
        public void AddToCart()
        {
            if (SelectedProduct != null)
            {
                var existingItem = CartItems.FirstOrDefault(item => item.ProductId == SelectedProduct.id);

                if (existingItem != null)
                {
                    // Увеличиваем количество и пересчитываем сумму
                    existingItem.Quantity++;
                    //existingItem.TotalPrice = existingItem.ProductPrice * existingItem.Quantity;
                }
                else
                {
                    // Добавляем новый товар в корзину
                    CartItems.Add(new Element_saleDto
                    {
                        ProductId = SelectedProduct.id,
                        ProductName = SelectedProduct.name,
                        ProductPrice = SelectedProduct.price,
                        Quantity = 1,
                        //TotalPrice = selectedProduct.price
                    });
                }

                // Уведомляем об изменении корзины
                OnPropertyChanged(nameof(CartItems));  // Обновить CartItems
                OnPropertyChanged(nameof(TotalSum));   // Обновить TotalSum
                
                var currentWindow = Application.Current.Windows.OfType<ADDElementSave>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
            }
        }
        public void AddToCart(ProductsDTO selectedProduct)
        {
            if (selectedProduct != null)
            {
                var existingItem = CartItems.FirstOrDefault(item => item.ProductId == selectedProduct.id);

                if (existingItem != null)
                {
                    // Увеличиваем количество и пересчитываем сумму
                    existingItem.Quantity++;
                    //existingItem.TotalPrice = existingItem.ProductPrice * existingItem.Quantity;
                }
                else
                {
                    // Добавляем новый товар в корзину
                    CartItems.Add(new Element_saleDto
                    {
                        ProductId = selectedProduct.id,
                        ProductName = selectedProduct.name,
                        ProductPrice = selectedProduct.price,
                        Quantity = 1,
                        //TotalPrice = selectedProduct.price
                    });
                }

                // Уведомляем об изменении корзины
                OnPropertyChanged(nameof(CartItems));  // Обновить CartItems
                OnPropertyChanged(nameof(TotalSum));   // Обновить TotalSum
            }
        }
        public ICommand RemoveItemCommand { get; }

        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }

        public void RemoveItem(Element_saleDto item)
        {
            if (item != null)
            {
                CartItems.Remove(item);  // Удаляем элемент из корзины
                OnPropertyChanged(nameof(TotalSum));  // Обновляем общую сумму
            }
        }
        public ICommand CleanCommand {  get; }
        public ICommand AddToNoomCommand { get; }
        public ICommand OpenAddElementSaleCommand { get; } // Команда для открытия окна
        public ICommand Creat { get; }
        public ICommand OpenNoomberCommand { get; }
        public ICommand OpenADDCientCommand {  get; }
      
        public SellerVM(long idsal)
        {
            _tableModel = new TableModel();
            //_productsModel = new ProductsModel();
            AddToCartCommand = new RelayCommand(AddToCart); // Инициализация команды добавления в корзину
            AddToNoomCommand = new Command<ClientDTO>(AddToNoom);
            OpenADDCientCommand = new RelayCommand(OpenADDClient);
            CleanCommand = new RelayCommand(Clean);
            OpenAddElementSaleCommand = new RelayCommand(OpenAddElementSale); // Инициализация команды открытия окна
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
            LoadProducts();

        }
        private void IncreaseQuantity(Element_saleDto item)
        {
            if (item != null)
            {
                item.Quantity++;
                OnPropertyChanged(nameof(TotalSum));  // Обновляем общую сумму
            }
        }

        // Метод для уменьшения количества товара
        private void DecreaseQuantity(Element_saleDto item)
        {
            if (item != null && item.Quantity > 1)
            {
                item.Quantity--;
                OnPropertyChanged(nameof(TotalSum));  // Обновляем общую сумму
            }
        }

        // Метод для удаления товара из корзины
       
        public void CreateOrder()
        {
            if (CartItems.Count == 0)
            {
                return;
            }

            // Создаем новый заказ
            var order = new SaleDTO() {
                cost = 0,
                salesmn_id = idsal,
                client_id = _client.id,
            };
            // Добавляем товары из корзины в заказ
            foreach (var item in CartItems)
            {
                order.cost += (long)item.TotalPrice;
            }
            var q = new SalyModel();
            var id =q.CreateSale(order);

            foreach (var item in CartItems)
            {
                var selectedElement = new Element_saleDto()
                {
                    SaleId = id,    // ID только что добавленной продажи
                    ProductId = item.ProductId,   // ID товара
                    Quantity = item.Quantity,     // Количество товара
                    price = (long)item.ProductPrice,    // Цена за единицу товара
                    //TotalPrice = item.TotalPrice, // Общая стоимость товара
                };

                // Используем ваш метод для сохранения элемента продажи
                var elementSaleModel = new Element_saleModel(); // Предполагаем, что у вас есть такой класс
                elementSaleModel.CreateElementSale(selectedElement);  // Сохраняем элемент продажи в БД
            }
            // Очищаем корзину после оформления заказа
            CartItems.Clear();

            // Уведомляем об изменении корзины
            OnPropertyChanged(nameof(CartItems));

        }
        private void LoadProducts()
        {
            //Products = _tableModel.GetProductsDTO(); // Получаем список доступных продуктов
            var productsFromDb = _tableModel.GetProductsDTO();
            Products = productsFromDb.ToList(); // Загружаем список продуктов из модели
            OnPropertyChanged(nameof(Products)); // Уведомляем об изменении свойства
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
        public void AddToNoom(ClientDTO selectedClient)
        {
            if (selectedClient != null)
            {
                _client = selectedClient;
                _phoneNumber = _client.noomber;

                OnPropertyChanged("PhoneNumber"); // Уведомляем об изменении свойства CartItems
            }
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
            _windowService.OpenWindow("Noomber", this);
        }
        public void End()
        {
            var mainWindow=new MainWindow();
            mainWindow.Show();
        }
        public void OpenAddElementSale()
        {
            _windowService.OpenWindow("ADDElementSave", this);
        }
        public void OpenADDClient()
        {
            _windowService.OpenWindow("ADDClient", this);
        }

     
        public void RefreshProducts()
        {
            IdFilter = null;
            SearchTerm = null;
            LoadProducts(); // Перезагружаем продукты из базы данных 
        }
        private void FindProducts()
        {
            var allProducts = _tableModel.GetProductsDTO(); // Получаем все продукты из базы данных

            // Фильтруем по названию, если SearchTerm не пустой
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                allProducts = _tableModel.GetProductsDTOName(SearchTerm, allProducts);
            }

            // Фильтруем по ID, если IdFilter задан
            if (IdFilter.HasValue)
            {
                allProducts = _tableModel.GetProductsDTOID(IdFilter.Value, allProducts);
            }

            // Обновляем список продуктов с отфильтрованными результатами
            Products = allProducts.ToList();
            OnPropertyChanged(nameof(Products)); // Уведомляем об изменении свойства
        }
        public void ResetFilters()
        {
            SearchTerm = string.Empty;  // Сбрасываем строку поиска по названию
            IdFilter = null;  // Сбрасываем фильтр по ID
            LoadProducts();  // Загружаем все продукты заново
        }
        private long? _idFilter;
        public long? IdFilter
        {
            get => _idFilter;
            set
            {
                _idFilter = value;
                OnPropertyChanged(nameof(IdFilter));
                FindProducts(); // Автоматически ищем при изменении фильтра ID
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
                FindProducts(); // Автоматически ищем при изменении термина
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

        public ICommand AddClientCommand { get; }

        public void AddClient()
        {
            var id=_clientModel.CreateClient(NewClientNumber);
            AddToNoom(_tableModel.GetClientDTOID(id));
        }
        private bool CanAddClient()
        {
            return !string.IsNullOrWhiteSpace(NewClientNumber);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}