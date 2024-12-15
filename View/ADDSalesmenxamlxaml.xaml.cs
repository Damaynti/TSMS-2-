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
    public partial class ADDSalesmenxamlxaml : Window
    {
        public ADDSalesmenxamlxaml(int mode)
        {
            InitializeComponent();
            if (mode == 1)
            {
                btn.SetBinding(Button.CommandProperty, new Binding("AddObjInDBCommand"));
                Up.Visibility= Visibility.Collapsed;
            }
            else
            {
                btn.SetBinding(Button.CommandProperty, new Binding("UpdObjInDBCommand"));
                Up.Visibility = Visibility.Visible;
            }
        }

    }
}
