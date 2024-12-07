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
using TSMS_2_.Services;
using TSMS_2_.ViewModel;

namespace TSMS_2_.View
{
    /// <summary>
    /// Логика взаимодействия для ADDElementSave.xaml
    /// </summary>
    public partial class ADDElementSave : Window
    {
        public ADDElementSave()
        {
            InitializeComponent();
            this.DataContext = new AddElementViewModel();
        }
        private readonly SellerVM _sellerViewModel;

        public ADDElementSave(SellerVM sellerViewModel)
        {
            InitializeComponent();
            _sellerViewModel = sellerViewModel; // Сохраняем ссылку на SellerVM
            this.DataContext = new AddElementViewModel(); // Устанавливаем контекст данных
        }

   

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (datagrid.SelectedItem is ProductsDTO selectedProduct) // Убедитесь, что имя 'dataGrid' совпадает
            {
                // Добавляем выбранный продукт в корзину
                _sellerViewModel.AddToCart(selectedProduct);
                this.Close(); // Закрываем окно после выбора продукта
            }
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            id.Text=null;
            Sname.Text=null;
        }
    }
}
