﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TSMS_2_.Services
{
    public interface IWindowService
    {
       
        void ShowWindow(string windowType, object viewModel = null);
        void OpenWindow(string windowType, object viewModel, int mode);
        void CloseWindow(Window window);
        void HideWindow(Window window);
        void SWindow(Window window);
    }
}
