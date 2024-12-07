using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TSMS_2_.Services
{
    public interface IWindowService
    {
       
        void ShowWindow(string windowType, object viewModel = null,long i=0);
        void OpenWindow(string windowType, object viewModel, int mode=0);
        void CloseWindow(Window window);
    }
}
