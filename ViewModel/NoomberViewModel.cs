using PdfSharp.Pdf.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TSMS_2_.DTO;
using TSMS_2_.EF;
using TSMS_2_.Model;

namespace TSMS_2_.ViewModel
{
    public class NoomberViewModel : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private string _searchNoom;
        private List<ClientDTO> _client;
        private ClientDTO _selectedClient;

        public NoomberViewModel() {
            _client = new List<ClientDTO>();
            LoadProducts();
        }
        private void LoadProducts()
        {
            var clientsFromDb = _tableModel.GetClientDTO();
            Clients = clientsFromDb.ToList(); // Загружаем список продуктов из модели
            OnPropertyChanged(nameof(Clients)); // Уведомляем об изменении свойства
        }
        public string SearchNoom
        {
            get => _searchNoom;
            set
            {
                _searchNoom = value;
                OnPropertyChanged(nameof(SearchNoom));
                FindProducts(); // Автоматически ищем при изменении термина
            }
        }
       
        public ClientDTO SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
            }
        }
        public List<ClientDTO> Clients
        {
            get => _client;
            set
            {
                if (_client != value)
                {
                    _client = value;
                    OnPropertyChanged(nameof(Clients));
                }
            }
        }
        private void FindProducts()
        {
            var allClient = _tableModel.GetClientDTO(); // Получаем все продукты из базы данных

            // Фильтруем по названию, если SearchTerm не пустой
            if (!string.IsNullOrEmpty(SearchNoom))
            {
                allClient = _tableModel.GetnClientDTONoom(SearchNoom,allClient);
            }

            // Обновляем список продуктов с отфильтрованными результатами
            Clients = allClient.ToList();
            OnPropertyChanged(nameof(Clients)); // Уведомляем об изменении свойства
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
