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
using TSMS_2_.EF;
using TSMS_2_.Services;
using TSMS_2_.ViewModel;

namespace TSMS_2_.View
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove(); // Allow dragging the window
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized; // Minimize the window
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized; // Toggle maximize/restore
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Close the application
        }

        private void ButtonAddSalesman_Click(object sender, RoutedEventArgs e)
        {
            // Logic to add a new salesman
            var viewModel = (AdminVM)DataContext;
            //viewModel.AddSalesman(); // Call the method in ViewModel to add a salesman
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            // Logic to save changes for the selected salesman
            var viewModel = (AdminVM)DataContext;
            viewModel.SaveChanges(); // Call the method in ViewModel to save changes
        }

        //private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    // Logic to handle selection changes in the DataGrid
        //    var viewModel = (SalesmanViewModel)DataContext;
        //    if (salGrid.SelectedItem is salesmanDTO selectedSalesman)
        //    {
        //        viewModel.SelectedSalesman = selectedSalesman; // Update the ViewModel with the selected salesman
        //    }
        //}
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }

}

