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
    /// Логика взаимодействия для SellerWindow.xaml
    /// </summary>
    public partial class SellerWindow : Window
    {
        
        public SellerWindow(long i)
        {
            InitializeComponent();
            this.DataContext = new SellerVM(i);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var _cont=this.DataContext as SellerVM;
            _cont.End();
            this.Close();
        }
    }
}
