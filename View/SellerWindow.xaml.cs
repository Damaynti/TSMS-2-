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

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Здесь вы можете обработать выбранный элемент
            if (dataGrid.SelectedItem is Element_saleDto selectedItem)
            {
                // Логика для работы с выбранным элементом
                MessageBox.Show($"Выбран товар: {selectedItem.ProductName}, Количество: {selectedItem.Quantity}");
            }
        }
    }
}
