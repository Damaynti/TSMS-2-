using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TSMS_2_.DTO;
using TSMS_2_.EF;
using TSMS_2_.Model;
using TSMS_2_.Services;

namespace TSMS_2_.ViewModel
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly ProductsModel _productsModel = new ProductsModel();
        private List<ProductsDTO> _products; // Список продуктов
        private readonly IWindowService _windowService;
        private ProductsDTO _selectedProduct;

        public ICommand AddProductCommand { get; }
        public ICommand UpdateProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand RefreshProductsCommand { get; }
        public ICommand UpdObjInDBCommand { get; }
        public ICommand AddObjInDBCommand { get; }
        public ICommand FindProductCommand { get; }
        public ICommand AddToCartCommand { get; } // Команда для добавления в корзину

        // Properties for Product details
        public string Name { get; set; }
        public long CategoryId { get; set; }
        public long Price { get; set; }
        public long? Count { get; set; }

        // Search properties
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

        // Корзина для хранения продуктов
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

        public ProductsViewModel()
        {
            _products = new List<ProductsDTO>();
            _windowService = new WindowService();
            AddObjInDBCommand = new RelayCommand(CreateProduct);
            UpdObjInDBCommand = new RelayCommand(UpdateProduct);
            AddProductCommand = new RelayCommand(OpenAddProduct);
            UpdateProductCommand = new RelayCommand(OpenUpdateProduct);
            DeleteProductCommand = new RelayCommand(DeleteSelectedProduct);
            RefreshProductsCommand = new RelayCommand(RefreshProducts);
            AddToCartCommand = new RelayCommand(AddToCart); // Инициализация команды добавления в корзину
            FindProductCommand = new RelayCommand(FindProducts);
            LoadProducts(); // Загружаем продукты из базы данных при инициализации
        }

        private void LoadProducts()
        {
            var productsFromDb = _tableModel.GetProductsDTO();
            Products = productsFromDb.ToList(); // Загружаем список продуктов из модели
            OnPropertyChanged(nameof(Products)); // Уведомляем об изменении свойства
        }

        private void FindProducts()
        {
            var allProducts = _tableModel.GetProductsDTO(); // Получаем все продукты из базы данных

            // Фильтруем по названию, если SearchTerm не пустой
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                allProducts = allProducts
                    .Where(p => p.name.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            // Фильтруем по ID, если IdFilter задан
            if (IdFilter.HasValue)
            {
                allProducts = allProducts
                    .Where(p => p.id == IdFilter.Value)
                    .ToList();
            }

            // Обновляем список продуктов с отфильтрованными результатами
            Products = allProducts.ToList();
            OnPropertyChanged(nameof(Products)); // Уведомляем об изменении свойства
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

        public List<ProductsDTO> Products
        {
            get => _products;
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged(nameof(Products));
                }
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


        // Open the window to add a new product

        public void OpenAddProduct()
        {
            SelectedProduct = new ProductsDTO(); // Сбрасываем выбранный продукт 
            _windowService.OpenWindow("AddProduct", this, 1);
        }

        // Open the window to update an existing product

        public void OpenUpdateProduct()
        {
            if (SelectedProduct != null)
            {
                SelectedProduct = new ProductsDTO(SelectedProduct); // Создаем копию для редактирования 
                _windowService.OpenWindow("AddProduct", this, 2);
            }
        }

        // Refresh the list of products from the database

        public void RefreshProducts()
        {
            LoadProducts(); // Перезагружаем продукты из базы данных 
        }

        // Delete the selected product

        private void DeleteSelectedProduct()
        {
            if (SelectedProduct != null)
            {
                _productsModel.DeleteProduct(SelectedProduct.id);
                Products.Remove(SelectedProduct); // Удаляем продукт из списка после удаления из базы данных 
                RefreshProducts(); // Обновляем список продуктов после удаления  
            }
        }

        // Update the selected product in the database

        private void UpdateProduct()
        {
            if (SelectedProduct != null)
            {
                _productsModel.UpdateProduct(SelectedProduct);
                RefreshProducts(); // Обновляем список продуктов после обновления в базе данных  
            }
        }


        // Create a new product in the database

        public void CreateProduct()
        {
            _productsModel.CreateProduct(SelectedProduct);
            RefreshProducts(); // Обновляем список продуктов после создания нового продукта в базе данных  
        }


        // Метод для добавления продукта в корзину

        public void AddToCart()
        {
            if (SelectedProduct != null)
            {
                Cart.Add(new ProductsDTO(SelectedProduct)); // Добавляем копию выбранного продукта в корзину
                OnPropertyChanged(nameof(Cart)); // Уведомляем об изменении свойства Cart
            }
        }


        // Метод для сброса фильтров

        public void ResetFilters()
        {
            SearchTerm = string.Empty;  // Сбрасываем строку поиска по названию
            IdFilter = null;  // Сбрасываем фильтр по ID
            LoadProducts();  // Загружаем все продукты заново
        }


        // Property changed event handler

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}