﻿using ConfigBottomView.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConfigBottomView
{
    /// <summary>
    /// Profile.xaml 的交互逻辑
    /// </summary>
    public partial class Profile : UserControl
    {
        public Profile()
        {
            InitializeComponent();

            this.DataContext = new HistogramViewModel(0);
        }

        private HistogramViewModel _histogramViewModel = new HistogramViewModel(0);
    }

       
    
}
