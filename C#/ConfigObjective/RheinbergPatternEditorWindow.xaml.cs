﻿using Global.Mode.Config;
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
using ThemeManager.Controls;

namespace ConfigObjective
{
    /// <summary>
    /// RheinbergPatternEditorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RheinbergPatternEditorWindow : BaseWindow
    {
        public List<RheinbergPattern> DefaultrheinbergPatterns =  new()
        {
            new RheinbergPattern() { Rheinberg0 = (SolidColorBrush)Brushes.Blue, Rheinberg1 = (SolidColorBrush)Brushes.Yellow, Rheinberg2 = (SolidColorBrush)Brushes.Yellow },
            new RheinbergPattern() { Rheinberg0 = (SolidColorBrush)Brushes.Blue, Rheinberg1 = (SolidColorBrush)Brushes.Red, Rheinberg2 = (SolidColorBrush)Brushes.Lime },
            new RheinbergPattern() { Rheinberg0 = (SolidColorBrush)Brushes.Black, Rheinberg1 = (SolidColorBrush)Brushes.Yellow, Rheinberg2 = (SolidColorBrush)Brushes.Blue },
            new RheinbergPattern() { Rheinberg0 = (SolidColorBrush)Brushes.Black, Rheinberg1 = (SolidColorBrush)Brushes.Yellow, Rheinberg2 = (SolidColorBrush)Brushes.Blue }
        };
        public List<RheinbergPattern> rheinbergPatterns;

        public RadioButton SelectRadionButton;
        public int SelectedIndex = 0;

        public List<RheinbergPattern> OldrheinbergPatterns;
        public int OldSelectedIndex = 0;

        public RheinbergPatternEditorWindow(List<RheinbergPattern> rheinbergPatterns, int SelectedIndex)
        {
            if (rheinbergPatterns!=null&& rheinbergPatterns.Count == 4)
            {
                this.rheinbergPatterns = rheinbergPatterns;
            }
            else
            {
                this.rheinbergPatterns = DefaultrheinbergPatterns;
            }
            OldrheinbergPatterns = rheinbergPatterns;
            OldSelectedIndex = SelectedIndex;
            InitializeComponent();

            this.SelectedIndex = SelectedIndex;

            SetSelectColor();
            if (SelectedIndex == 0)
            {
                RadioButton1.IsChecked = true;
                SelectRadionButton = RadioButton1;
            }
            else if (SelectedIndex == 1)
            {
                RadioButton2.IsChecked = true;
                SelectRadionButton = RadioButton2;

            }
            else if (SelectedIndex == 2)
            {
                RadioButton3.IsChecked = true;
                SelectRadionButton = RadioButton3;

            }
            else if (SelectedIndex == 3)
            {
                RadioButton4.IsChecked = true;
                SelectRadionButton = RadioButton4;
            }
        }

        public RheinbergPatternEditorWindow()
        {
            this.rheinbergPatterns = DefaultrheinbergPatterns;
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            SetSelectColor();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Default_Click(object sender, RoutedEventArgs e)
        {
            this.rheinbergPatterns = DefaultrheinbergPatterns;
            SelectedIndex = 0;
            SetSelectColor();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (OldrheinbergPatterns != null)
            {
                SelectedIndex = OldSelectedIndex;
                rheinbergPatterns = OldrheinbergPatterns;
            }
            else
            {
                this.rheinbergPatterns = DefaultrheinbergPatterns;
                SelectedIndex = 0;
            }

            SetSelectColor();

            if (SelectedIndex == 0)
            {
                SelectRadionButton = RadioButton1;
            }
            else if (SelectedIndex == 1)
            {
                SelectRadionButton = RadioButton2;
            }
            else if (SelectedIndex == 2)
            {
                SelectRadionButton = RadioButton3;
            }
            else if (SelectedIndex == 3)
            {
                SelectRadionButton = RadioButton4;
            }

            this.Close();
        }

        private void SetSelectColor()
        {
            ColorPciker10.SelectColor = rheinbergPatterns[0].Rheinberg0;
            ColorPciker11.SelectColor = rheinbergPatterns[0].Rheinberg1;
            ColorPciker20.SelectColor = rheinbergPatterns[1].Rheinberg0;
            ColorPciker21.SelectColor = rheinbergPatterns[1].Rheinberg1;
            ColorPciker22.SelectColor = rheinbergPatterns[1].Rheinberg2;
            ColorPciker30.SelectColor = rheinbergPatterns[2].Rheinberg0;
            ColorPciker31.SelectColor = rheinbergPatterns[2].Rheinberg1;
            ColorPciker32.SelectColor = rheinbergPatterns[2].Rheinberg2;
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            rheinbergPatterns[0].Rheinberg0 = ColorPciker10.SelectColor;
            rheinbergPatterns[0].Rheinberg1 = ColorPciker11.SelectColor;
            rheinbergPatterns[0].Rheinberg2 = ColorPciker11.SelectColor;
            rheinbergPatterns[1].Rheinberg0 = ColorPciker20.SelectColor;
            rheinbergPatterns[1].Rheinberg1 = ColorPciker21.SelectColor;
            rheinbergPatterns[1].Rheinberg2 = ColorPciker22.SelectColor;
            rheinbergPatterns[2].Rheinberg0 = ColorPciker30.SelectColor;
            rheinbergPatterns[2].Rheinberg1 = ColorPciker31.SelectColor;
            rheinbergPatterns[2].Rheinberg2 = ColorPciker32.SelectColor;
        }

        private void RadioButton1_Checked(object sender, RoutedEventArgs e)
        {
            SelectedIndex = 0;
        }

        private void RadioButton2_Checked(object sender, RoutedEventArgs e)
        {
            SelectedIndex = 1;  
        }

        private void RadioButton3_Checked(object sender, RoutedEventArgs e)
        {
            SelectedIndex = 2;
        }

        private void RadioButton4_Checked(object sender, RoutedEventArgs e)
        {
            SelectedIndex = 3;
        }


    }


}
