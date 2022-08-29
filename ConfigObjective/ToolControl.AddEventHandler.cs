﻿using Global.UserControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Lambda;
using System.Windows.Input;
using Global.Mode.Config;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Global.Common.Extensions;
using ConfigObjective.UserControls;

namespace ConfigObjective
{
	public partial class ToolControl
	{
		
		private void AddEventHandler()
		{
			LambdaControl.AddLambdaEventHandler("STOP_ACQUIRE", STOP_ACQUIRE, false);		
		}

		private bool STOP_ACQUIRE(object sender, EventArgs e)
		{
			//MessageBox.Show("1");
            var mulDimensional = windowData.MulDimensional;
			mulDimensional.mulDimensionalAreas.Clear();
			mulDimensional.mulDimensionalPoints.Clear();
			var spot = new Global.Mode.Config.Spot();
            foreach (var item in Map.selectedPoints)
            {
                List<int> selectedpoint = new List<int>() { (int)item.X, (int)item.Y };
                spot.Includes.Add(selectedpoint);
            }

            TestMean testMean = new TestMean();
			testMean.Spot = spot;

			List<string> Mode = new();
			if (checkbox51.IsChecked == true)
				Mode.Add("bright-field");
			if (checkbox52.IsChecked == true)
				Mode.Add("dark-field");
			if (checkbox53.IsChecked == true)
				Mode.Add("rheinberg");
			if (checkbox54.IsChecked == true)
				Mode.Add("relief-contrast");
			if (checkbox55.IsChecked == true)
				Mode.Add("quantitative-phase");
			if (checkbox56.IsChecked == true)
				Mode.Add("phase-contrast");

			testMean.Dimensional = new Global.Mode.Config.Dimensional() { Mode = Mode };


			Global.Mode.Config.TimeWiseSerial timeWiseSerial = new Global.Mode.Config.TimeWiseSerial();
			Global.Mode.Config.Optimized optimized = new Optimized();
			optimized.Optimize = mulDimensional.OptimizedSel;
			// focus-mode
			if (mulDimensional.OptimizedSel)
			{
				optimized.IsGlobal = mulDimensional.Optimized.IsGlobal;
				optimized.IsLocal = mulDimensional.Optimized.IsLocal;
				optimized.Global = mulDimensional.Optimized.Global;
				optimized.Local = mulDimensional.Optimized.Local;
				optimized.Precision = mulDimensional.Optimized.Precision;
			}
			else if (!mulDimensional.OptimizedSel)
			{
				
				optimized.IsGlobal = mulDimensional.UserDefined.IsGlobal;
				optimized.IsLocal = mulDimensional.UserDefined.IsLocal;
				optimized.Global = mulDimensional.UserDefined.Global;
				optimized.Local = mulDimensional.UserDefined.Local;
				optimized.Precision = mulDimensional.UserDefined.Precision;
			}
			 Global.Mode.Config.Twise twise = new Twise();
			// t-wise
			if (mulDimensional.TFirst)
            {
				twise.Interval = "first";

			}
			else if (mulDimensional.TEvery)
            {
				twise.Interval = "every";
			}
            else if (mulDimensional.TN)
            {
				twise.Interval = mulDimensional.TWiseN.ToString();

			}
			else if (mulDimensional.TwiseEnable)
			{
				twise.Interval = "null";

			}
			testMean.Dimensional.Focusmode.Twise = twise;
			Global.Mode.Config.Pwise pwise = new Pwise();
			//p-wise
			if (mulDimensional.PFirst)
			{
				pwise.Interval = "first";

			}
			else if (mulDimensional.PEvery)
			{
				pwise.Interval = "every";
			}
			else if (mulDimensional.PN)
			{
				pwise.Interval = mulDimensional.PWiseN.ToString();

			}
			else if (mulDimensional.PwiseEnable)
            {
				pwise.Interval = "null";
			}

			testMean.Dimensional.Focusmode.Pwise = pwise;




			testMean.Dimensional.Focusmode.Optimized = optimized;

			timeWiseSerial.Times = mulDimensional.TNumberEnable ? -1 : mulDimensional.TNumber;
			timeWiseSerial.Duration = mulDimensional.TIntervalEnable ? -1 : mulDimensional.TInterval;

			switch (mulDimensional.TIntervalUnits)
			{
				
				case "秒":
					timeWiseSerial.Mode = "s";
					break;
				case "分钟":
					timeWiseSerial.Mode = "min";
					break;
				case "小时":
					timeWiseSerial.Mode = "h";
					break;
				case "天":
					timeWiseSerial.Mode = "d";
					break;

				default: /* 可选的 */
					timeWiseSerial.Mode = "s";
					break;
			}

			


		   testMean.Dimensional.TimeWiseSerial = timeWiseSerial;

			Global.Mode.Config.ZstackWiseSerial zstackWiseSerial = new Global.Mode.Config.ZstackWiseSerial();
			zstackWiseSerial.ZStep = mulDimensional.Zstep;
			zstackWiseSerial.ZBegin = mulDimensional.ZStart;
			zstackWiseSerial.ZEnd = mulDimensional.ZEnd;
			zstackWiseSerial.ZAbsolute = mulDimensional.ZAbsolute;
			testMean.Dimensional.ZstackWiseSerial = zstackWiseSerial;

			var Dimensions = new StringBuilder();

			Dimensions.Append("xy");
			if (ToggleButton503.IsChecked == true)
				Dimensions.Append('z');
			if (ToggleButton504.IsChecked == true)
				Dimensions.Append('t');
			if (ToggleButton506.IsChecked == true)
				Dimensions.Append('p');
			if (ToggleButton505.IsChecked == true)
				Dimensions.Append("edof");


			testMean.Dimensional.Dimensions = Dimensions.ToString();
			testMean.Dimensional.Savedir = windowData.SolutionDir;
			
			testMean.Stage = windowData.Stage;
			windowData.Config.Dimensional = testMean.Dimensional;
			windowData.Config.Spot = testMean.Spot;
			windowData.Config.Stage = testMean.Stage;

           // System.Windows.MessageBox.Show(testMean.ToJson());

			if (testMean.Dimensional.Mode.Count == 0)
            {
                
                var r = Global.Common.MessageBox1.Show("请选择成像模式!");

                // System.Windows.MessageBox.Show("请选择成像模式", "请选择成像模式",MessageBoxButton.OK);

                LambdaControl.Trigger("COLLECTION_COMPLETED", this, new Dictionary<string, object>() { });
			}
            else
            {
				//Start Collection need to startalive first
				Window mainwin = Application.Current.MainWindow;
				Grid grid = (Grid)mainwin.FindName("stageAcquisition");
				if (grid != null)
				{
					DockPanel dockPanel = (DockPanel)grid.Children[1];
					ToggleButton toggleAlive = (ToggleButton)dockPanel.Children[0];
					StackPanel stackPanel = (StackPanel)dockPanel.Children[1];
					ToggleButton toggleAcquire = (ToggleButton)stackPanel.Children[0];
					
					if (toggleAlive != null && toggleAlive.IsChecked == false)
						{

							toggleAlive.IsChecked = true;
							toggleAlive.Content = "停止预览";
							LambdaControl.Trigger("STOP_ALIVE", this, new Dictionary<string, object>() { });
							EventArgs eventArgs = new EventArgs();
					}
					
				}
				LambdaControl.Trigger("START_ACQUIRE1", this, testMean.ToJson());


		}
			
			return true;
        }
	}
}
