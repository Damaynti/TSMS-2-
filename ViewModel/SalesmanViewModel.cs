
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TSMS_2_.DTO;
using TSMS_2_.Model;
using TSMS_2_.Services;
using TSMS_2_.EF;
using TSMS_2_.View;
using System.Windows;
using System.Data.Entity;

namespace TSMS_2_.ViewModel
{
    public class SalesmanViewModel : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly SalesmanModel _salesmanModel = new SalesmanModel();
        private List<salesmanDTO> _salesmen;
        private readonly IWindowService _windowService;
        private salesmanDTO _selectedSalesman;
        private List<salesmanDTO> _allSalesmen;
        private string _selectedFilter;
        private string _searchQuery;

        public ICommand UpdObjInDBCommand { get; }
        public ICommand AddObjInDBCommand { get; }
        public ICommand AddObjCommand { get; }
        public ICommand DeleteObjCommand { get; }
        public ICommand RefreshObjCommand { get; }
        public ICommand EditObjCommand { get; }
        public ICommand EndCommand { get; }
        public ICommand SearchCommand { get; }

        public SalesmanViewModel()
        {
            _salesmen = new List<salesmanDTO>();
            _windowService = new WindowService();
            AddObjInDBCommand = new RelayCommand(CreateSalesman);
            UpdObjInDBCommand = new RelayCommand(UpdateSalesman);
            AddObjCommand = new RelayCommand(OpenAddSalesman);
            EditObjCommand = new RelayCommand(OpenUpdSalesman);
            DeleteObjCommand = new RelayCommand(DeleteSelectedSalesman);
            RefreshObjCommand = new RelayCommand(RefreshSalesmen);
            EndCommand = new RelayCommand(End);
            SearchCommand = new RelayCommand(ExecuteSearch);

            Filters = new List<string> { "Все", "Работают", "Уволены" };
            SelectedFilter = "Все";
            SearchQuery = string.Empty;

            LoadSalesmen();
        }

        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (_selectedFilter != value)
                {
                    _selectedFilter = value;
                    OnPropertyChanged(nameof(SelectedFilter));
                    SearchQuery = string.Empty; // Обнуляем строку поиска при изменении фильтра
                    ApplyFilter();
                }
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged(nameof(SearchQuery));
                    if (!string.IsNullOrWhiteSpace(_searchQuery))
                    {
                        SelectedFilter = "Все"; // Сбрасываем фильтр при изменении строки поиска
                    }
                }
            }
        }

        public List<string> Filters { get; }

        public List<salesmanDTO> Salesmen
        {
            get => _salesmen;
            set
            {
                if (_salesmen != value)
                {
                    _salesmen = value;
                    OnPropertyChanged(nameof(Salesmen));
                }
            }
        }

        public salesmanDTO SelectedSalesman
        {
            get => _selectedSalesman;
            set
            {
                _selectedSalesman = value;
                OnPropertyChanged(nameof(SelectedSalesman));
            }
        }

        private void LoadSalesmen()
        {
            using (var db = new Model1())
            {
                db.salesman.Load();
                _allSalesmen = db.salesman
                    .Where(i => !i.admin) // Exclude admin
                    .ToList()
                    .Select(i => new salesmanDTO(i))
                    .ToList();
            }
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                if (SelectedFilter == "Работают")
                    Salesmen = _allSalesmen.Where(s => s._work == "Работает").ToList();
                else if (SelectedFilter == "Уволены")
                    Salesmen = _allSalesmen.Where(s => s._work == "Уволен").ToList();
                else
                    Salesmen = _allSalesmen;
            }
        }

        private void ExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                Salesmen = _allSalesmen; // Показываем всех сотрудников
            }
            else
            {
                var filteredSalesmen = _allSalesmen
                    .Where(s => s.FullName.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                Salesmen = filteredSalesmen.Any() ? filteredSalesmen : new List<salesmanDTO>();
            }
        }

        public void OpenAddSalesman()
        {
            ResetSearchAndFilter();
            SelectedSalesman = new salesmanDTO();
            _windowService.OpenWindow("ADDSalesman", this, 1);
        }

        public void OpenUpdSalesman()
        {
            if (SelectedSalesman != null && SelectedSalesman.id != 0)
            {
               

                SelectedSalesman = new salesmanDTO(SelectedSalesman);
                _windowService.OpenWindow("ADDSalesman", this, 2);
            }
        }

        private void ResetSearchAndFilter()
        {
            SearchQuery = string.Empty;
            SelectedFilter = "Все";
            LoadSalesmen();
        }

        public void RefreshSalesmen()
        {
            var updatedSalesmen = _tableModel.GetSalesmanDTO();
            Salesmen.Clear();
            Salesmen = updatedSalesmen;
            OnPropertyChanged(nameof(Salesmen));
        }

        private void DeleteSelectedSalesman()
        {
            if (SelectedSalesman != null)
            {
                _salesmanModel.DeleteSalesman(SelectedSalesman.id);
                Salesmen.Remove(SelectedSalesman);
                RefreshSalesmen();
            }
        }

        private void UpdateSalesman()
        {
            if (IsSalesmanValid(SelectedSalesman))
            {
                _salesmanModel.UpdateSalesman(SelectedSalesman);
                RefreshSalesmen();
                End();
            }
            ResetSearchAndFilter();
        }

        private void CreateSalesman()
        {
            if (IsSalesmanValid(SelectedSalesman))
            {
                _salesmanModel.CreateSalesman(SelectedSalesman);
                RefreshSalesmen();
                End();
            }
        }

        private bool IsSalesmanValid(salesmanDTO salesman)
        {
            return !string.IsNullOrEmpty(salesman.FullName) &&
                   !string.IsNullOrEmpty(salesman.mail) &&
                   !string.IsNullOrEmpty(salesman.password) &&
                   !string.IsNullOrEmpty(salesman.number) &&
                   salesman.salary > 0 &&
                   !string.IsNullOrEmpty(salesman.address);
        }

        public void End()
        {
            var currentWindow = Application.Current.Windows.OfType<ADDSalesmenxamlxaml>().FirstOrDefault();
            _windowService.CloseWindow(currentWindow);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
