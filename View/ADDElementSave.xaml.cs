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
    public partial class ADDElementSave : Window
    {
        public ADDElementSave(int mode)
        {
            InitializeComponent();
            if (mode == 1)
            {
                btn.Visibility = Visibility.Collapsed;
                p.Header = "Цена";
                p.Binding =new Binding("price") {Mode=BindingMode.OneWay};
            }
            else
            {
                btn.Visibility = Visibility.Visible;
                p.Header = "Закупка";
                p.Binding = new Binding("purchase") { Mode = BindingMode.OneWay };
            }
        }
    }
}
