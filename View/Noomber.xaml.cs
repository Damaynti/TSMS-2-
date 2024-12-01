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
using TSMS_2_.ViewModel;

namespace TSMS_2_.View
{
    /// <summary>
    /// Логика взаимодействия для Noomber.xaml
    /// </summary>
    public partial class Noomber : Window
    {
        public Noomber()
        {
            InitializeComponent();
            this.DataContext = new NoomberViewModel();
        }
    }
}
