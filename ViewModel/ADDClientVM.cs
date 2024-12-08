//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using TSMS_2_.DTO;
//using TSMS_2_.EF;
//using TSMS_2_.Model;

//namespace TSMS_2_.ViewModel
//{
//    public class ADDClientVM : INotifyPropertyChanged
//    {
//        private string _newClientNumber;
//        private readonly ClientModel _clientModel = new ClientModel();
//        private readonly TableModel _tableModel = new TableModel();
//        public string NewClientNumber
//        {
//            get => _newClientNumber;
//            set
//            {
//                _newClientNumber = value;
//                OnPropertyChanged();
//            }
//        }

//        public ICommand AddClientCommand { get; }

//        public ADDClientVM()
//        {
//            //AddClientCommand = new RelayCommand(AddClient, CanAddClient);
//        }

//        private bool CanAddClient()
//        {
//            return !string.IsNullOrWhiteSpace(NewClientNumber);
//        }

//        public ClientDTO AddClient()
//        {
//           var id= _clientModel.CreateClient(NewClientNumber);
//           return _tableModel.GetClientDTOID(id);
//        }


//        public event PropertyChangedEventHandler PropertyChanged;

//        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}

