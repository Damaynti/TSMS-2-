using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TSMS_2_.DTO;
using TSMS_2_.ViewModel;

namespace TSMS_2_.View
{
    /// <summary>
    /// Логика взаимодействия для ADDClient.xaml
    /// </summary>
    public partial class ADDClient : Window
    {
        private readonly SellerVM _sellerViewModel;

        public ADDClient(SellerVM sellerViewModel)
        {
            InitializeComponent();
            _sellerViewModel = sellerViewModel;
            this.DataContext = sellerViewModel; // Устанавливаем контекст данных
        }

      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
                //_sellerViewModel.AddToNoom(_sellerViewModel.AddClient());
                this.Close(); // Закрываем окно после выбора продукта
            
        }
    }
}
