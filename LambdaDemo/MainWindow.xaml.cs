﻿using LambdaManager;
using LambdaManager.Core;
using LambdaManager.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace LambdaDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILambdaUI
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void LoadControl(string name, string lib, string mount)
        {
            Assembly assembly = Assembly.LoadFile(Directory.GetCurrentDirectory() + "/" + lib);
            string fullName = lib.Replace(".dll", "") + "." + name;

            if ((assembly.CreateInstance(fullName) is Control control ))
            {
                if (control is Window window)
                {
                    MenuItem menuItem = AddMenuItem(mount);
                    menuItem.Click += delegate
                    {
                        window.Owner = this;
                        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        window.ShowDialog();
                    };
                }
                else
                {
                    stackpanel1.Children.Add(control);
                }

            }
        }
        public MenuItem? AddMenuItem(string path)
        {
            ItemCollection items = menu.Items;
            if (items == null)
            {
                return null;
            }
            MenuItem last = null;
            string[] array = path.Split("/");
            foreach (string name in array)
            {
                bool found = false;
                foreach (MenuItem item in (IEnumerable)items)
                {
                    if (item.Header.ToString() == name)
                    {
                        found = true;
                        items = item.Items;
                        break;
                    }
                }
                if (!found)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = name;
                    items.Add(menuItem);
                    items = menuItem.Items;

                    last = menuItem;
                }
            }
            return last;
        }



        private void Window_Initialized(object sender, EventArgs e)
        {
            ConfigLibrary ConfigLibrary = new ConfigLibrary();
            ConfigLibrary.lambdaUI = this;
            ConfigLibrary.Load();
            ConfigLibrary.InitializeLibrary();
            ConfigLibrary.LoadUIComponents();

            mainView.Children.Clear();
            mainView.Children.Add(ViewGrid.mainView);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Common.CommonExit();
            Environment.Exit(-1);
        }

        public void LoadMenuCommand(string name, List<string> raises)
        {

        }
    }


}
