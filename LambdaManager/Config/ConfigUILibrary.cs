using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using LambdaManager.Core;
using LambdaManager.DataType;
using Quartz;

namespace LambdaManager.Config;

public class UIPlugin
{
	public string filePath { get; set; }
    public string typeName { get; set; }

	public string MD5 { get; set; }

	public Control control { get; set; }
    public Side side { get; set; }

}



public class ConfigUILibrary: ILambdaUI
{

	public List<UIPlugin> UIPlugins = new List<UIPlugin>() { };


    public MainWindow Main { get; set; }

	public ConfigUILibrary(MainWindow Main)
	{
		this.Main = Main;

    }


    public void LoadControl(string name, string lib, string mount)
	{
        string typeName = lib.Replace(".dll", "") + "." + name;
        UIPlugin uIPlugin = new UIPlugin() { filePath = Directory.GetCurrentDirectory() + "\\" + lib, typeName = typeName };

        byte[] dllbytes = File.ReadAllBytes(uIPlugin.filePath);
        Assembly assembly = Assembly.Load(dllbytes);

        Side side = GetConfigSide(mount);
        if (side == Side.MENU)
        {
            string menuPath = GetMenuPath(mount);
            LoadMenuDialog(assembly, typeName, menuPath);
        }
        else
        {
            uIPlugin.side = side;
            UIPlugins.Add(uIPlugin);
            LoadConfigPanel(uIPlugin);
        }
    }

    public static string GetMD5(string path)
    {
        var hash = MD5.Create();
        var stream = new FileStream(path, FileMode.Open);
        byte[] hashByte = hash.ComputeHash(stream);
        stream.Close();
        return BitConverter.ToString(hashByte).Replace("-", "");
    }


    public void LoadConfigPanel(UIPlugin uIPlugin)
    {
        StackPanel stackPanel = (StackPanel)Main.GetConfigPanel(uIPlugin.side);

		uIPlugin.MD5 = GetMD5(uIPlugin.filePath);
        byte[] dllbytes = File.ReadAllBytes(uIPlugin.filePath);
        Assembly assembly = Assembly.Load(dllbytes);
        if ((assembly.CreateInstance(uIPlugin.typeName) is Control control))
        {
            stackPanel.Children.Add(control);
        }
    }

	private void LoadMenuDialog(Assembly assembly, string typeName, string path)
	{
		Assembly assembly2 = assembly;
		MenuItem menu = Main.AddMenuItem(path);
		if (menu != null)
		{
			menu.Click += delegate
			{
				if ((assembly2.CreateInstance(typeName) is Window window))
				{
                    window.Owner = Main;
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.ShowDialog();
                }
			};
		}
		else
		{

		}
	}

	public void LoadMenuCommand(string name, List<string> raises)
	{
		if (name == null || raises == null)
		{
			return;
		}
		MenuItem menu = Main.AddMenuItem(name);
		if (menu != null)
		{
			menu.Click += delegate
			{
				foreach (string item in raises)
				{
					Common.CallEvent(item, IntPtr.Zero);
				}
			};
		}
		else
		{
		}
	}

	private static Side GetConfigSide(string mount)
	{
		string[] tokens = mount.Split(':');
		return (Side)Enum.Parse(typeof(Side), tokens[0].Trim(), ignoreCase: true);
    }


	private static string GetMenuPath(string mount)
	{
		return mount.Split(':')[1].Trim();
	}

}
