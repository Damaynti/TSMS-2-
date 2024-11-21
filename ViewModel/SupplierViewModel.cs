using System;
using System.Collections.Generic;
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
    public class SupplierViewModel : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly SupplierModel _supplierModel = new SupplierModel();
        private List<SupplierDTO> _suppliers;
        private readonly IWindowService _windowService;
        private SupplierDTO _selectedSupplier;

        public ICommand UpdObjInDBCommand { get; }
        public ICommand AddObjInDBCommand { get; }
        public ICommand AddSupplierCommand { get; }
        public ICommand UpdateSupplierCommand { get; }
        public ICommand DeleteSupplierCommand { get; }
        public ICommand RefreshSuppliersCommand { get; }

        // Properties for Supplier details
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Mail { get; set; }
        public string Requisites { get; set; }
        public string Number { get; set; }

        public SupplierViewModel()
        {
            _suppliers = new List<SupplierDTO>();
            _windowService = new WindowService();
            AddObjInDBCommand = new RelayCommand(CreateSupplier);
            UpdObjInDBCommand = new RelayCommand(UpdateSupplier);
            AddSupplierCommand = new RelayCommand(OpenAddSupplier);
            UpdateSupplierCommand = new RelayCommand(OpenUpdateSupplier);
            DeleteSupplierCommand = new RelayCommand(DeleteSelectedSupplier);
            RefreshSuppliersCommand = new RelayCommand(RefreshSuppliers);
        }

        // List of suppliers
        public List<SupplierDTO> Suppliers
        {
            get
            {
                if (_suppliers == null || _suppliers.Count == 0)
                {
                    Suppliers = _tableModel.GetSupplierDTO(); // Получаем список поставщиков из модели
                    OnPropertyChanged(nameof(Suppliers));
                }
                return _suppliers;
            }
            set
            {
                if (_suppliers != value)
                {
                    _suppliers = value;
                    OnPropertyChanged(nameof(Suppliers));
                }
            }
        }

        public SupplierDTO SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
            }
        }

        // Load suppliers from the database
       

        // Open the window to add a new supplier
        public void OpenAddSupplier()
        {
            SelectedSupplier = new SupplierDTO(); // Сбрасываем выбранного поставщика
            _windowService.OpenWindow("AddSupplier", this, 1);
        }

        // Open the window to update an existing supplier
        public void OpenUpdateSupplier()
        {
            if (SelectedSupplier != null)
            {
                SelectedSupplier = new SupplierDTO(SelectedSupplier); // Создаем копию для редактирования
                _windowService.OpenWindow("AddSupplier", this, 2);
            }
        }

        // Refresh the list of suppliers
        public void RefreshSuppliers()
        {
            var us = _tableModel.GetSupplierDTO();
            Suppliers.Clear();
            Suppliers = _tableModel.GetSupplierDTO();
            OnPropertyChanged("Suppliers");
        }

        // Delete the selected supplier
        private void DeleteSelectedSupplier()
        {
            if (SelectedSupplier != null)
            {
                _supplierModel.DeleteSupplier(SelectedSupplier.id);
                Suppliers.Remove(SelectedSupplier);
                RefreshSuppliers();
            }
        }

        // Update the selected supplier in the database
        private void UpdateSupplier()
        {
            if (SelectedSupplier != null)
            {
                _supplierModel.UpdateSupplier(SelectedSupplier);
                RefreshSuppliers();
            }
        }

        // Create a new supplier in the database
        public void CreateSupplier()
        {
            _supplierModel.CreateSupplier(SelectedSupplier);
            RefreshSuppliers();
        }

        // Property changed event handler
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
