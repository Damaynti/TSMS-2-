﻿using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TSMS_2_.View;
using TSMS_2_.ViewModel;

namespace TSMS_2_.Services
{
    public class WindowService:IWindowService
    {
        public void ShowWindow(string windowType, object viewModel)
        {
            Window window;

            switch (windowType)
            {
                case "AdminWindow":
                    window = new AdminWindow();
                    window.DataContext = viewModel;
                    break;
                case "Noomber":
                    window = new Noomber();
                    window.DataContext = viewModel;
                    break;
                case "SellerWindow":
                    window = new SellerWindow();
                    window.DataContext = viewModel;
                    break;
                case "ADDSup":
                    window = new SupAddSup();
                    window.DataContext = viewModel;
                    break;
                case "MainWindow":
                    window = new MainWindow();
                    break;
                
                default:
                    throw new ArgumentException("Unknown window type");
            }
            window.Show();
        }
        public void OpenWindow(string windowType, object vM, int mode)
        {
            Window window;

            switch (windowType)
            {
                case "ADDSalesman":
                    window = new ADDSalesmenxamlxaml(mode);
                    break;
                case "AddProduct":
                    window = new ADDProduct(mode);
                    break;
                case "AddSupplier":
                    window = new ADDSupplier(mode);
                    break;
                case "ADDElementSave":
                    window = new ADDElementSave(mode);
                    break;
                case "AddSale":
                    window = new ADDSale(mode);
                    break;
                case "ADDClient":
                    window = new ADDClient(mode);
                    break;
                case "ADDLoanAgreement":
                    window = new ADDLoanAgreement(mode);
                    break;
                case "ADDSupply":
                    window = new AddSupply(mode);
                    break;
                case "AddCategory":
                    window = new AddCategory(mode);
                    break;
                default:
                    throw new ArgumentException("Unknown window type");
            }
            window.DataContext = vM;
            window.ShowDialog();
        }
        public void CloseWindow(Window window)
        {
            if (window != null)
            {
                
                window.Close();
            }
        }
        public void HideWindow(Window window)
        {
            if (window != null)
            {

                window.Hide();
            }
        }

        public void SWindow(Window window)
        {
            if (window != null)
            {

                window.Show();
            }
        }
    }
}

