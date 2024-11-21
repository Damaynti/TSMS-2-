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
using TSMS_2_.Services;

namespace TSMS_2_.ViewModel
{
    public class SupplyViewModel
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly SupplyModel _supplyModel = new SupplyModel();
        private List<SupplyDTO> _supplies;
        private readonly IWindowService _windowService;
        private SupplyDTO _selectedSupply;

        public ICommand AddSupplyCommand { get; }
        public ICommand UpdateSupplyCommand { get; }
        public ICommand DeleteSupplyCommand { get; }
        public ICommand RefreshSuppliesCommand { get; }

        // Properties for Supply details
        public long SupplierId { get; set; }
        public DateTime Data { get; set; }
        public long Cost { get; set; }

        public SupplyViewModel()
        {
            _supplies = new List<SupplyDTO>();
            _windowService = new WindowService();
            AddSupplyCommand = new RelayCommand(CreateSupply);
            UpdateSupplyCommand = new RelayCommand(UpdateSelectedSupply);
            DeleteSupplyCommand = new RelayCommand(DeleteSelectedSupply);
            RefreshSuppliesCommand = new RelayCommand(RefreshSupplies);
        }

        // List of supplies
        public List<SupplyDTO> Supplies
        {
            get
            {
                if (_supplies == null || _supplies.Count == 0)
                {
                    LoadSupplies();
                }
                return _supplies;
            }
            set
            {
                if (_supplies != value)
                {
                    _supplies = value;
                    OnPropertyChanged(nameof(Supplies));
                }
            }
        }

        public SupplyDTO SelectedSupply
        {
            get => _selectedSupply;
            set
            {
                _selectedSupply = value;
                OnPropertyChanged(nameof(SelectedSupply));
            }
        }

        // Load supplies from the database
        private void LoadSupplies()
        {
            Supplies = _tableModel.GetSupplyDTO(); // Получаем список поставок из модели
            OnPropertyChanged(nameof(Supplies));
        }

        // Open the window to add a new supply
        public void OpenAddSupply()
        {
            SelectedSupply = new SupplyDTO(); // Сбрасываем выбранную поставку
            _windowService.OpenWindow("AddSupply", this, 1);
        }

        // Open the window to update an existing supply
        public void OpenUpdateSupply()
        {
            if (SelectedSupply != null)
            {
                SelectedSupply = new SupplyDTO(SelectedSupply); // Создаем копию для редактирования
                _windowService.OpenWindow("AddSupply", this, 2);
            }
        }

        // Refresh the list of supplies
        public void RefreshSupplies()
        {
            LoadSupplies();
            OnPropertyChanged(nameof(Supplies));
        }

        // Delete the selected supply
        private void DeleteSelectedSupply()
        {
            if (SelectedSupply != null)
            {
                _supplyModel.DeleteSupply(SelectedSupply.id);
                Supplies.Remove(SelectedSupply);
                RefreshSupplies();
            }
        }

        // Update the selected supply in the database
        private void UpdateSelectedSupply()
        {
            if (SelectedSupply != null)
            {
                _supplyModel.UpdateSupply(SelectedSupply);
                RefreshSupplies();
            }
        }

        // Create a new supply in the database
        public void CreateSupply()
        {
            if (SelectedSupply != null)
            {
                _supplyModel.CreateSupply(SelectedSupply);
                RefreshSupplies();
            }
        }

        // Property changed event handler
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
