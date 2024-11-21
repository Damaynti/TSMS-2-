using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TSMS_2_.View;

namespace TSMS_2_.Services
{
    public class WindowService:IWindowService
    {
        public void ShowWindow(string windowType, object viewModel,long i)
        {
            Window window;

            switch (windowType)
            {
                case "AdminWindow":
                    window = new AdminWindow();
                    window.DataContext = viewModel;
                    break;
                case "SellerWindow":
                    window = new SellerWindow(i);
                    break;
                //case "ADDElementSave":
                //    window = new ADDElementSave();
                //    break;
                //case "AddObj":
                //    window = new AddObjWindow();
                //    window.DataContext = viewModel;
                //    break;
                //case "EditObj":
                //    window = new EditObjectWindow();
                //    window.DataContext = viewModel;
                //    break;
                default:
                    throw new ArgumentException("Unknown window type");
            }

            //window.DataContext = viewModel;
            window.Show();
        }
        public void OpenWindow(string windowType, object vM, int mode)
        {
            Window window;

            switch (windowType)
            {
                case "ADDSalesman":
                    if (mode == 1)
                    {
                        window = new ADDSalesmenxamlxaml(1, vM);
                    }
                    else
                    {
                        window = new ADDSalesmenxamlxaml(2, vM);
                    }
                    break;
                case "AddProduct":
                    if(mode == 1)
                    {
                        window = new ADDProduct(1, vM);
                    }
                    else
                    {
                        window = new ADDProduct(2, vM);
                    }
                    break;
                case "AddSupplier":
                    if (mode == 1)
                    {
                        window = new ADDSupplier(1, vM);
                    }
                    else
                    {
                        window = new ADDSupplier(2, vM);
                    }
                    break;
                case "AddSale":
                    if (mode == 1)
                    {
                        window = new ADDSale(1, vM);
                    }
                    else
                    {
                        window = new ADDSale(2, vM);
                    }
                    break;
                case "ADDLoanAgreement":
                    if (mode == 1)
                    {
                        window = new ADDLoanAgreement(1, vM);
                    }
                    else
                    {
                        window = new ADDLoanAgreement(2, vM);
                    }
                    break;
                default:
                    throw new ArgumentException("Unknown window type");
            }

            window.ShowDialog();
            //window.ShowDialog();
        }
        public void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}

