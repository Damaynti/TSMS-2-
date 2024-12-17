﻿using System;
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
    /// Логика взаимодействия для AddSupply.xaml
    /// </summary>
    public partial class AddSupply : Window
    {
        public AddSupply(int mode)
        {
            InitializeComponent();

        }
        private void Window_Closed(object sender, EventArgs e)
        {
            var viewModel = (SupplyViewModel)this.DataContext;
            viewModel.Show();
        }

    }
}
