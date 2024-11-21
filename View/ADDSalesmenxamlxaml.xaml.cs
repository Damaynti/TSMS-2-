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

namespace TSMS_2_.View
{
    /// <summary>
    /// Логика взаимодействия для ADDSalesmenxamlxaml.xaml
    /// </summary>
    public partial class ADDSalesmenxamlxaml : Window
    {
        public ADDSalesmenxamlxaml(int mode, object vM)
        {
            InitializeComponent();
            if (mode == 1)
            {
                this.DataContext = vM;
                btn.SetBinding(Button.CommandProperty, new Binding("AddObjInDBCommand"));
            }
            else
            {
                this.DataContext = vM;
                btn.SetBinding(Button.CommandProperty, new Binding("UpdObjInDBCommand"));
            }
        }
        public ADDSalesmenxamlxaml() { InitializeComponent(); }



        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Закрытие окна
            this.Close();
        }

       
       
    }
}
