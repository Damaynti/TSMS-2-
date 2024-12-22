using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TSMS_2_.DTO;
using TSMS_2_.EF;
using TSMS_2_.Model;
using TSMS_2_.Services;
using TSMS_2_.View;

namespace TSMS_2_.ViewModel
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly ClientModel _clientModel = new ClientModel();
        private List<ClientDTO> _clients;
        private ClientDTO _selectedClient;
        private readonly IWindowService _windowService;

        public ICommand AddNewClientCommand { get; }
        public ICommand AddClientCommand { get; }
        public ICommand EditClientCommand { get; }
        public ICommand UpdateClientCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand RefreshClientsCommand { get; }

        public ClientViewModel()
        {
            _clients = new List<ClientDTO>();
            _windowService = new WindowService();

            EditClientCommand = new RelayCommand(EditClient);
            AddClientCommand = new RelayCommand(OpenAddClient);
            UpdateClientCommand = new RelayCommand(OpenUpdateClient);
            DeleteClientCommand = new RelayCommand(DeleteSelectedClient);
            RefreshClientsCommand = new RelayCommand(LoadClients);
            AddNewClientCommand = new RelayCommand(AddClient);

            LoadClients();
        }

        private void EditClient()
        {
            if (SelectedClient != null)
            {
                if (!Regex.IsMatch(SelectedClient.noomber, @"^(\+7|8)\d{10}$"))
                {
                    MessageBox.Show(
                        "Номер телефона некорректен. Убедитесь, что он содержит только цифры и соответствует российскому формату (например, +79123456789 или 89123456789).",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (_tableModel.DoesClientNumberExist(SelectedClient.noomber, SelectedClient.id))
                {
                    MessageBox.Show(
                        "Клиент с таким номером телефона уже существует в базе данных.",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }
                if (SelectedClient.tClient != "Физическое лицо")
                {
                    SelectedClient.physical_person = false;
                }
                else SelectedClient.physical_person = true;
                
                _clientModel.UpdateClient(SelectedClient);

                MessageBox.Show(
                    "Информация о клиенте успешно обновлена.",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                var currentWindow = Application.Current.Windows.OfType<ADDClient>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
                LoadClients();
            }
            else
            {
                MessageBox.Show(
                    "Выберите клиента для изменения.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void AddClient()
        {
            if (SelectedClient != null && !string.IsNullOrWhiteSpace(SelectedClient.noomber))
            {
                if (!Regex.IsMatch(SelectedClient.noomber, @"^(\+7|8)\d{10}$"))
                {
                    MessageBox.Show(
                        "Номер телефона некорректен. Убедитесь, что он содержит только цифры и соответствует российскому формату (например, +79123456789 или 89123456789).",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (_tableModel.DoesClientNumberExist(SelectedClient.noomber))
                {
                    MessageBox.Show(
                        "Клиент с таким номером телефона уже существует в базе данных.",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }
                if (SelectedClient.tClient != "Физическое лицо")
                {
                    SelectedClient.physical_person = false;
                }
                else SelectedClient.physical_person = true;
                if (SelectedClient.name == null) SelectedClient.name = "неизвестный";
                var idD = _tableModel.FindDiscountIdByPurchaseAmount(SelectedClient.purchase_amount);
                if (idD != null)
                {
                    SelectedClient.discount_id = (long)idD;
                }
                SelectedClient.id = _clientModel.CreateClient(SelectedClient);

                MessageBox.Show(
                    "Клиент успешно добавлен.",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                var currentWindow = Application.Current.Windows.OfType<ADDClient>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
                LoadClients();
            }
            else
            {
                MessageBox.Show(
                    "Не заполнены обязательные поля (номер телефона).",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        public List<ClientDTO> Clients
        {
            get => _clients;
            set
            {
                if (_clients != value)
                {
                    _clients = value;
                    OnPropertyChanged(nameof(Clients));
                }
            }
        }

        public ClientDTO SelectedClient
        {
            get => _selectedClient;
            set
            {
                if (_selectedClient != value)
                {
                    _selectedClient = value;
                    OnPropertyChanged(nameof(SelectedClient));
                }
            }
        }

        private void LoadClients()
        {
            var clientsFromDb = _tableModel.GetClientsDTO();
            Clients = clientsFromDb.ToList();
        }

        private void OpenAddClient()
        {
            SelectedClient = new ClientDTO { physical_person = true }; 
            _windowService.OpenWindow("ADDClient", this, 1);
        }

        private void OpenUpdateClient()
        {
            if (SelectedClient != null && SelectedClient.id != 0)
            {
                SelectedClient = new ClientDTO(SelectedClient);
                _windowService.OpenWindow("ADDClient", this, 2);
            }
        }

        private void DeleteSelectedClient()
        {
            if (SelectedClient != null)
            {
                _clientModel.RemoveClientAssociations(SelectedClient.id);
                _clientModel.DeleteClient(SelectedClient.id);
                LoadClients();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
