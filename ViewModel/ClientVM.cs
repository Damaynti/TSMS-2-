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

namespace TSMS_2_.ViewModel
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        private readonly TableModel _tableModel = new TableModel();
        private readonly ClientModel _clientModel = new ClientModel();
        private List<ClientDTO> _clients;
        private ClientDTO _selectedClient;
        private readonly IWindowService _windowService;

        public ICommand AddClientCommand { get; }
        public ICommand UpdateClientCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand RefreshClientsCommand { get; }

        public ClientViewModel()
        {
            _clients = new List<ClientDTO>();
            _windowService = new WindowService();

            AddClientCommand = new RelayCommand(OpenAddClient);
            UpdateClientCommand = new RelayCommand(OpenUpdateClient);
            DeleteClientCommand = new RelayCommand(DeleteSelectedClient);
            RefreshClientsCommand = new RelayCommand(LoadClients);

            LoadClients();
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
            SelectedClient = new ClientDTO();
            _windowService.OpenWindow("AddClient", this, 1);
        }

        private void OpenUpdateClient()
        {
            if (SelectedClient != null)
            {
                SelectedClient = new ClientDTO(SelectedClient);
                _windowService.OpenWindow("UpdateClient", this, 2);
            }
        }

        private void DeleteSelectedClient()
        {
            if (SelectedClient != null)
            {
                _clientModel.DeleteClient(SelectedClient.id);
                Clients.Remove(SelectedClient);
                LoadClients();
            }
        }

        private void SaveClient()
        {
            if (!string.IsNullOrWhiteSpace(SelectedClient.name) && !string.IsNullOrWhiteSpace(SelectedClient.noomber))
            {
                if (SelectedClient.id == 0)
                {
                    _clientModel.CreateClient(SelectedClient);
                }
                else
                {
                    _clientModel.UpdateClient(SelectedClient);
                }

                LoadClients();
                var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
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
