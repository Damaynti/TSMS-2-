using System;
using System.Collections.Generic;
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

namespace TSMS_2_.ViewModel
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly ProductsModel _productsModel = new ProductsModel();
        private List<ProductsDTO> _products; 
        private readonly IWindowService _windowService;
        private ProductsDTO _selectedProduct;
        public ICommand AddProductCommand { get; }
        public ICommand UpdateProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand UpdObjInDBCommand { get; }
        public ICommand AddObjInDBCommand { get; }

        public ProductsViewModel()
        {
            _products = new List<ProductsDTO>();
            _windowService = new WindowService();
            AddObjInDBCommand = new RelayCommand(CreateProduct);
            UpdObjInDBCommand = new RelayCommand(UpdateProduct);
            AddProductCommand = new RelayCommand(OpenAddProduct);
            UpdateProductCommand = new RelayCommand(OpenUpdateProduct);
            DeleteProductCommand = new RelayCommand(DeleteSelectedProduct);
            LoadProducts(); 
        }

        private void LoadProducts()
        {
            var productsFromDb = _tableModel.GetProductsDTO();
            Products = productsFromDb.ToList(); 
            OnPropertyChanged(nameof(Products)); 
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

        public void OpenAddProduct()
        {
            SelectedProduct = new ProductsDTO();  
            _windowService.OpenWindow("AddProduct", this, 1);
        }

        public void OpenUpdateProduct()
        {
            if (SelectedProduct != null)
            {
                SelectedProduct = new ProductsDTO(SelectedProduct); 
                _windowService.OpenWindow("AddProduct", this, 2);
            }
        }
        private void DeleteSelectedProduct()
        {
            if (SelectedProduct != null)
            {
                _productsModel.DeleteProduct(SelectedProduct.id);
                Products.Remove(SelectedProduct);
                LoadProducts();
            }
        }
        private void UpdateProduct()
        {
            if (SelectedProduct.name != null && SelectedProduct.categories != null && SelectedProduct.price != 0 && SelectedProduct.count != null)
            {
                _productsModel.UpdateProduct(SelectedProduct);
                LoadProducts();
            }
            var currentWindow = Application.Current.Windows.OfType<ADDProduct>().FirstOrDefault();
            _windowService.CloseWindow(currentWindow);
        }
        public void CreateProduct()
        {
            if (SelectedProduct.name != null && SelectedProduct.categories!=null && SelectedProduct.price!=0 && SelectedProduct.count!=null)
            {
                _productsModel.CreateProduct(SelectedProduct);
                LoadProducts();
                var currentWindow = Application.Current.Windows.OfType<ADDProduct>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}