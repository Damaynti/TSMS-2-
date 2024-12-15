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
    public class CategoriesViewModel : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly CategoryModel _categoryModel = new CategoryModel();
        private List<CategoryDto> _categories;
        private readonly IWindowService _windowService;
        private CategoryDto _selectedCategory;

        public ICommand AddCategoryCommand { get; }
        public ICommand UpdateCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand SaveCategoryCommand { get; }
        public ICommand UpCategoryCommand { get; }

        public CategoriesViewModel()
        {
            _categories = new List<CategoryDto>();
            _windowService = new WindowService();
            AddCategoryCommand = new RelayCommand(OpenAddCategory);
            UpdateCategoryCommand = new RelayCommand(OpenUpdateCategory);
            DeleteCategoryCommand = new RelayCommand(DeleteSelectedCategory);
            SaveCategoryCommand = new RelayCommand(SaveCategory);
            UpCategoryCommand=new RelayCommand(UpdateCat);
            LoadCategories();
        }
        private void UpdateCat()
        {
            if (SelectedCategory.Name != null )
            {
                _categoryModel.UpdateCategory(SelectedCategory);
                LoadCategories();
            }
            var currentWindow = Application.Current.Windows.OfType<AddCategory>().FirstOrDefault();
            _windowService.CloseWindow(currentWindow);
        }
        private void LoadCategories()
        {
            var categoriesFromDb = _tableModel.GetCategoriesDTO();
            Categories = categoriesFromDb.ToList();
            OnPropertyChanged(nameof(Categories));
        }

        public List<CategoryDto> Categories
        {
            get => _categories;
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged(nameof(Categories));
                }
            }
        }

        public CategoryDto SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        public void OpenAddCategory()
        {
            SelectedCategory = new CategoryDto();
            _windowService.OpenWindow("AddCategory", this,1);
        }

        public void OpenUpdateCategory()
        {
            if (SelectedCategory != null)
            {
                SelectedCategory = new CategoryDto
                {
                    Id = SelectedCategory.Id,
                    Name = SelectedCategory.Name,
                    Products = SelectedCategory.Products
                };
                _windowService.OpenWindow("AddCategory", this, 2);
            }
        }

        private void DeleteSelectedCategory()
        {
            if (SelectedCategory != null)
            {
                _categoryModel.DeleteCategory(SelectedCategory.Id);
                Categories.Remove(SelectedCategory);
                LoadCategories();
            }
        }

        private void SaveCategory()
        {
            if (!string.IsNullOrWhiteSpace(SelectedCategory.Name))
            {
                if (SelectedCategory.Id == 0)
                {
                    _categoryModel.CreateCategory(SelectedCategory);
                }
                else
                {
                    _categoryModel.UpdateCategory(SelectedCategory);
                }
                LoadCategories();
                CloseCurrentWindow();
            }
        }

        private void CloseCurrentWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddCategory>().FirstOrDefault();
            if (currentWindow != null)
            {
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
