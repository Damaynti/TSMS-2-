using PdfSharp.Pdf.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TSMS_2_.DTO;
using TSMS_2_.Model;

namespace TSMS_2_.ViewModel
{
    
    public class AddElementViewModel : INotifyPropertyChanged
    {

        private readonly TableModel _tableModel = new TableModel();
        public ICommand FindProductCommand { get; }
        private List<ProductsDTO> _products;
        private ProductsDTO _selectedProduct;
        public ICommand RefreshProductsCommand { get; }
        public ICommand AddToCartCommand { get; }
        public AddElementViewModel()
        {
            _products = new List<ProductsDTO>();
            RefreshProductsCommand = new RelayCommand(RefreshProducts);
            FindProductCommand = new RelayCommand(FindProducts);
            AddToCartCommand = new RelayCommand(AddToCart);
            LoadProducts();
        }
        public void RefreshProducts()
        {
            LoadProducts(); // Перезагружаем продукты из базы данных 
        }
        private void FindProducts()
        {
            var allProducts = _tableModel.GetProductsDTO(); // Получаем все продукты из базы данных
           
            // Фильтруем по названию, если SearchTerm не пустой
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                allProducts = _tableModel.GetProductsDTOName(SearchTerm,allProducts);
            }

            // Фильтруем по ID, если IdFilter задан
            if (IdFilter.HasValue)
            {
                allProducts=_tableModel.GetProductsDTOID(IdFilter.Value,allProducts);
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
        private void LoadProducts()
        {
            var productsFromDb = _tableModel.GetProductsDTO();
            Products = productsFromDb.ToList(); // Загружаем список продуктов из модели
            OnPropertyChanged(nameof(Products)); // Уведомляем об изменении свойства
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

        public void AddToCart()
        {
            if (SelectedProduct != null)
            {
                Cart.Add(new ProductsDTO(SelectedProduct)); // Добавляем копию выбранного продукта в корзину
                OnPropertyChanged(nameof(Cart)); // Уведомляем об изменении свойства Cart
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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
