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
using TSMS_2_.DTO;
using TSMS_2_.EF;
using TSMS_2_.Services;
using TSMS_2_.ViewModel;

namespace TSMS_2_.View
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }
        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TabItem tabItem && tabItem.DataContext is loanAgreementVM vm)
            {
                if (vm.LoadDataCommand.CanExecute(null))
                {
                    vm.LoadDataCommand.Execute(null);
                }
            }
        }


    }
}

