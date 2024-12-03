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
        private readonly SellerVM _sellerViewModel;

        public Noomber(SellerVM sellerViewModel)
        {
            InitializeComponent();
            _sellerViewModel = sellerViewModel; // Сохраняем ссылку на SellerVM
            this.DataContext = new NoomberViewModel(); // Устанавливаем контекст данных
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Sname.Text = null;
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {

            if (datagrid.SelectedItem is ClientDTO SelectedClient) // Убедитесь, что имя 'dataGrid' совпадает
            {
                // Добавляем выбранный продукт в корзину
                _sellerViewModel.AddToNoom(SelectedClient);
                this.Close(); // Закрываем окно после выбора продукта
            }
        }
    }
}
