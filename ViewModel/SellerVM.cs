using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private readonly ProductsModel _productsModel; // Модель для работы с продуктами
        private readonly TableModel _tableModel;
        private List<ProductsDTO> _products; // Список доступных продуктов
        private ProductsDTO _selectedProduct; // Выбранный продукт

        // Корзина для хранения товаров
        private ObservableCollection<Element_saleDto> _cartItems; // Изменено на ObservableCollection
        public ObservableCollection<Element_saleDto> CartItems
        {
            get => _cartItems ?? (_cartItems = new ObservableCollection<Element_saleDto>());
            set
            {
                _cartItems = value;
                OnPropertyChanged(nameof(CartItems));
            }
        }
        long idsal;

        public ICommand AddToCartCommand { get; }
        public ICommand OpenAddElementSaleCommand { get; } // Команда для открытия окна
        public ICommand Creat { get; }
        public ICommand OpenNoomberCommand { get; }
        public SellerVM(long idsal)
        {
            _tableModel = new TableModel();
            _productsModel = new ProductsModel();
            AddToCartCommand = new Command<ProductsDTO>(AddToCart); // Инициализация команды добавления в корзину
            OpenAddElementSaleCommand = new RelayCommand(OpenAddElementSale); // Инициализация команды открытия окна
            Creat = new RelayCommand(CreateOrder);
            OpenNoomberCommand = new RelayCommand(OpenNoomber);

            LoadProducts(); // Загружаем продукты при инициализации
            this.idsal = idsal;
        }
       
        public void CreateOrder()
        {
            if (CartItems.Count == 0)
            {
                return;
            }

            // Создаем новый заказ
            var order = new SaleDTO();
            order.cost = 0;
            order.salesmn_id = idsal;
            order.client_id = 4;
            // Добавляем товары из корзины в заказ
            foreach (var item in CartItems)
            {
                order.cost += (long)item.TotalPrice;
            }
            var q = new SalyModel();
            q.CreateSale(order);

            // Очищаем корзину после оформления заказа
            CartItems.Clear();

            // Уведомляем об изменении корзины
            OnPropertyChanged(nameof(CartItems));

        }
        private void LoadProducts()
        {
            Products = _tableModel.GetProductsDTO(); // Получаем список доступных продуктов
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
     
        public void AddToCart(ProductsDTO selectedProduct)
        {
            if (selectedProduct != null)
            {
                var existingItem = CartItems.FirstOrDefault(item => item.ProductId == selectedProduct.id);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                    existingItem.TotalPrice=existingItem.ProductPrice*existingItem.Quantity;

                    // Увеличиваем количество, если товар уже в корзине
                }
                else
                {
                    CartItems.Add(new Element_saleDto
                    {
                        ProductId = selectedProduct.id,
                        ProductName = selectedProduct.name,
                        ProductPrice = selectedProduct.price,
                        Quantity = 1,
                        TotalPrice= selectedProduct.price
                    });
                }

                OnPropertyChanged("CartItems"); // Уведомляем об изменении свойства CartItems
            }
        }
        public void OpenNoomber()
        {
            var noomberWindows = new Noomber();
            noomberWindows.Show();
        }
        public void OpenAddElementSale()
        {
            var addElementSaleWindow = new ADDElementSave(this); // Создание экземпляра окна ADDElementSale
            addElementSaleWindow.ShowDialog(); // Открываем окно как диалог
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}