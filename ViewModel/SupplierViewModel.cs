using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TSMS_2_.DTO;
using TSMS_2_.EF;
using TSMS_2_.Model;
using TSMS_2_.Services;
using TSMS_2_.View;

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
        public ICommand EndCommand { get; }
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
            EndCommand = new RelayCommand(End);
        }
        public void End()
        {
            var currentWindow = Application.Current.Windows.OfType<ADDSupplier>().FirstOrDefault();
            _windowService.CloseWindow(currentWindow);
        }
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
        public void OpenAddSupplier()
        {
            SelectedSupplier = new SupplierDTO(); 
            _windowService.OpenWindow("AddSupplier", this, 1);
        }
        public void OpenUpdateSupplier()
        {
            if (SelectedSupplier != null && SelectedSupplier.id!=0)
            {
                SelectedSupplier = new SupplierDTO(SelectedSupplier); 
                _windowService.OpenWindow("AddSupplier", this, 2);
            }
        }
        public void RefreshSuppliers()
        {
            Suppliers = _tableModel.GetSupplierDTO();
            OnPropertyChanged(nameof(Suppliers));
        }
        private void DeleteSelectedSupplier()
        {
            if (SelectedSupplier != null)
            {
                _supplierModel.DeleteSupplier(SelectedSupplier.id);
                Suppliers.Remove(SelectedSupplier);
                RefreshSuppliers();
            }
        }
        private void UpdateSupplier()
        {
            if (SelectedSupplier.mail != null && SelectedSupplier.FullName != null && SelectedSupplier.CompanyName != null && SelectedSupplier.address!=null && SelectedSupplier.number!=null && SelectedSupplier.requisites!=null)
            {
                _supplierModel.UpdateSupplier(SelectedSupplier);
                RefreshSuppliers();
                End();
            }
        }
        public void CreateSupplier()
        {
            if (SelectedSupplier.mail != null && SelectedSupplier.FullName != null && SelectedSupplier.CompanyName != null && SelectedSupplier.address != null && SelectedSupplier.number != null && SelectedSupplier.requisites != null)
            {
                _supplierModel.CreateSupplier(SelectedSupplier);
                RefreshSuppliers();
                End ();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
