using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TSMS_2_.ViewModel
{
    public class Element_saleViewModel : INotifyPropertyChanged
    {
        private readonly Element_saleModel _elementSaleModel = new Element_saleModel();
        private readonly IWindowService _windowService;
        private readonly TableModel _tableModel = new TableModel();
        private ObservableCollection<Element_saleDto> _saleElements;
        private Element_saleDto _selectedElement;
      
        public long CurrentSaleId { get; set; } // Идентификатор текущей продажи

        public ObservableCollection<Element_saleDto> SaleElements
        {
            get => _saleElements;
            set
            {
                if (_saleElements != value)
                {
                    _saleElements = value;
                    OnPropertyChanged(nameof(SaleElements));
                }
            }
        }

        public Element_saleDto SelectedElement
        {
            get => _selectedElement;
            set
            {
                _selectedElement = value;
                OnPropertyChanged(nameof(SelectedElement));
            }
        }

        public ICommand AddElementCommand { get; }
        public ICommand AddObjCommand { get; }
        public ICommand UpdateElementCommand { get; }
        public ICommand DeleteElementCommand { get; }
        public ICommand RefreshElementsCommand { get; }

        public Element_saleViewModel(long saleId)
        {
            CurrentSaleId = saleId; // Устанавливаем идентификатор текущей продажи
            _windowService = new WindowService();
            SaleElements = new ObservableCollection<Element_saleDto>();
            AddElementCommand = new RelayCommand(CreateElement);
            UpdateElementCommand = new RelayCommand(UpdateElement);
            DeleteElementCommand = new RelayCommand(DeleteSelectedElement);
            RefreshElementsCommand = new RelayCommand(RefreshElements);
            AddObjCommand = new RelayCommand(OpenAddSalesman);
            LoadSaleElements(); // Загружаем элементы продажи при инициализации
        }

        public Element_saleViewModel()
        {
            _windowService = new WindowService();
            SaleElements = new ObservableCollection<Element_saleDto>();
            AddElementCommand = new RelayCommand(CreateElement);
            UpdateElementCommand = new RelayCommand(UpdateElement);
            DeleteElementCommand = new RelayCommand(DeleteSelectedElement);
            RefreshElementsCommand = new RelayCommand(RefreshElements);
            AddObjCommand = new RelayCommand(OpenAddSalesman);
            LoadSaleElements(); // Загружаем элементы продажи при инициализации
        }

        public void OpenAddSalesman()
        {
            SelectedElement = new Element_saleDto(); // Reset the selected salesman
            _windowService.ShowWindow("ADDElementSave");
        }
        private void LoadSaleElements()
        {
            var elements = _tableModel.GetElementsBySaleId(CurrentSaleId); // Получаем элементы по идентификатору продажи
            SaleElements.Clear();
            foreach (var element in elements)
            {
                SaleElements.Add(element);
            }
        }

        private void CreateElement()
        {
            if (SelectedElement != null)
            {
                _elementSaleModel.CreateElementSale(SelectedElement);
                RefreshElements();
            }
        }

        private void UpdateElement()
        {
            if (SelectedElement != null)
            {
                _elementSaleModel.UpdateElementSale(SelectedElement);
                RefreshElements();
            }
        }

        private void DeleteSelectedElement()
        {
            if (SelectedElement != null)
            {
                _elementSaleModel.DeleteElementSale(SelectedElement.Id);
                SaleElements.Remove(SelectedElement);
                RefreshElements();
            }
        }

        private void RefreshElements()
        {
            LoadSaleElements(); // Обновляем элементы продажи
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
