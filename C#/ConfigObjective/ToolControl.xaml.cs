using Global;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Lambda;
using ConfigObjective.UserControls;
using System.Windows.Input;
using Global.Mode.Config;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Data;
using Global.Common.Util;
using Global.Common;
using System.Timers;

namespace ConfigObjective
{

    public partial class ToolControl : UserControl
    {
        WindowData windowData;
        public ToolControl()
        {
            windowData = WindowData.GetInstance();
            Update.UpdateEvent += UpdateGlobal;
            InitializeComponent();
        }

        bool IsFirstUpdate = true;
        private void UpdateGlobal()
        {
            if (!IsFirstUpdate)
            {
                IsFirstUpdate = false;
                MessageBox1.Show("根据参数更新","Grid");
            }
            CameraSetting_Update();
            OperatingMode_Update();
            Stage_Update();
            MulDimensional_Update();
        }

        /// <summary>
        /// 初始化写在前面
        /// </summary>
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            AddEventHandler();
            //初始化硬件

            ObjectiveSetting_Initialize();
            OperatingMode_Initialize();
            CameraSetting_Initialize();
        

            Map.moveButton.DataContext = windowData.mapModel;
            Stage_Initialize();
            MulDimensional_Initialize();

            Update.UpdateGlobal();
            IsFirstUpdate = true;

            ComboBox1.ItemsSource = windowData.deviceInformation.CameraResolution;


            #region slider
            SliderAbbreviation(Slider311, "BRIGHT_FIELD_DIAMETER", "diameter");
            //照明内径
            SliderAbbreviation(Slider321, "DARK_FIELD_INNER", "inner");
            //照明外径
            SliderAbbreviation(Slider322, "DARK_FIELD_OUTER", "outer");


            //背景校正
            ToggleButtonAbbreviation(Button322, "DARK _FIELD _BG_COLLECTION", "collection");
            Button322.IsChecked = false;


            //伽马
            //SliderAbbreviation1(Slider335, "RHEIN_BERG_GAMMA", "gamma");

            //差分

            //对比度
            SliderAbbreviation1(Slider341, "RELIEF_CONTRAST_CONTRAST", "contrast");
            //增益
            SliderAbbreviation1(Slider342, "RELIEF_CONTRAST_GAIN", "gain");
            //明场权重
            SliderAbbreviation1(Slider343, "RELIEF_CONTRAST_BF_WEIGHT", "weight");
            //测试附加
            //外径
            SliderAbbreviation(Slider344, "RELIEF_CONTRAST_OUTER", "outer");
            //内径
            SliderAbbreviation(Slider345, "RELIEF_CONTRAST_INNER", "inner");
            //Gamma
            SliderAbbreviation1(Slider346, "RELIEF_CONTRAST_GAMMA", "gamma");
            //相衬权重
            SliderAbbreviation1(Slider347, "RELIEF_CONTRAST_DP_WEIGHT", "weight");


            //相位

            //正则化参数
            // SliderAbbreviation1(Slider351, "QUANTITATIVE_PHASE_REG", "regularization");
           
            //Min
            SliderAbbreviation1(Slider353, "QUANTITATIVE_PHASE_MIN", "min");
            // Max
            SliderAbbreviation1(Slider354, "QUANTITATIVE_PHASE_MAX", "max");
            // Gamma
            SliderAbbreviation1(Slider355, "QUANTITATIVE_PHASE_GAMMA", "gamma");

            //相差

            //相差滤波
            SliderAbbreviation1(Slider361, "PHASE_CONTRAST_FILTER", "filter");
            //明场权重
            SliderAbbreviation1(Slider364, "PHASE_CONTRAST_BF_WEIGHT", "weight");
            //伽马
            SliderAbbreviation1(Slider365, "PHASE_CONTRAST_GAMMA", "gamma");
            //相差权重
            SliderAbbreviation1(Slider366, "PHASE_CONTRAST_PC_WEIGHT", "weight");

            #endregion


        }

        bool IsFirstLoad = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsFirstLoad && this.Parent is StackPanel stackPanel && stackPanel.Parent is Viewbox viewbox)
            {
                stackPanel.Margin = new Thickness(2, 2, 2, 0);
                viewbox.Width = double.NaN;
                IsFirstLoad = false;
                if (Reg.ReadValue("Software\\Grid", "InitializeStage"))
                {
                    WaitContorl.GetInstance().Show();
                    WaitContorl.GetInstance().textBox.Text = "位移台校准中";
                    WaitContorl.GetInstance().ShowTimeOut = 60000;
                    LambdaControl.Dispatch("STAGE_SETTING_RESET", this, new Dictionary<string, object> { });
                }
            }
        }









        //修改滑块的时候会触发两次，所以在这里加一个Tigger用来屏蔽掉第二次的触发，这里得(未来)重写。
        bool sliderfirst = true;
        /// <summary>
        /// SliderAbbreviation(int)
        /// </summary>
        private Slider SliderAbbreviation(Slider slider, string TriggerName, string TriggerParameter)
        {
            slider.ValueChanged += delegate (object sender, RoutedPropertyChangedEventArgs<double> e)
            {

                if (!windowData.ACQUIRE)
                {
                    Dictionary<string, object> data = new() { { TriggerParameter, (int)slider.Value } };
                    LambdaControl.Trigger(TriggerName, slider, data);
                }
                else
                {
                    if (sliderfirst)
                    {
                        var result = MessageBox.Show("是否修改当前多维采集设置", "显微镜", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.No)
                        {
                            sliderfirst = false;
                            slider.Value = e.OldValue;
                        }
                    }
                    else
                    {
                        sliderfirst = true;
                    }
                }

            };
            return slider;
        }

        /// <summary>
        /// SliderAbbreviation(double)
        /// </summary>
        private Slider SliderAbbreviation1(Slider slider, string TriggerName, string TriggerParameter)
        {
            slider.ValueChanged += delegate (object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!windowData.ACQUIRE)
                {
                    if (slider.Name == "Slider351")
                    {
                        Dictionary<string, object> data = new() { { TriggerParameter, (double)slider.Value*0.01 } };
                        LambdaControl.Trigger(TriggerName, slider, data);
                    }
                    else
                    {
                        Dictionary<string, object> data = new() { { TriggerParameter, (double)slider.Value } };
                        LambdaControl.Trigger(TriggerName, slider, data);
                    }

                }
                else
                {
                    if (sliderfirst)
                    {
                        var result = MessageBox.Show("是否修改当前多维采集设置", "显微镜", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.No)
                        {
                            sliderfirst = false;
                            slider.Value = e.OldValue;
                        }
                    }
                    else
                    {
                        sliderfirst = true;
                    }
                }

            };
            return slider;
        }

        /// <summary>
        /// ToggleButton缩写优化
        /// </summary>
        /// <param name="FindName"></param>
        /// <param name="TriggerName"></param>
        /// <param name="TriggerParameter"></param>
        private ToggleButton ToggleButtonAbbreviation(ToggleButton toggleButton, string TriggerName, string TriggerParameter)
        {
            toggleButton.Checked += delegate
            {
                Dictionary<string, object> data = new Dictionary<string, object>() { { TriggerParameter, toggleButton.IsChecked } };
                LambdaControl.Trigger(TriggerName, toggleButton, data);
            };
            toggleButton.Unchecked += delegate
            {
                Dictionary<string, object> data = new() { { TriggerParameter, toggleButton.IsChecked } };
                LambdaControl.Trigger(TriggerName, toggleButton, data);
            };
            return toggleButton;
        }

        private void ToggleButton222_Checked(object sender, RoutedEventArgs e)
        {
           // windowData.MulDimensional.TInterval = ">>";
        }

        private void ToggleButton221_Checked(object sender, RoutedEventArgs e)
        {
             windowData.MulDimensional.TNumber = -1;
        }

        private void ToggleButton504_Checked(object sender, RoutedEventArgs e)
        {
           if (!(bool)ToggleButton503.IsChecked)
            {
                ZwisePanel.Visibility = Visibility.Hidden;
            }
        }

        private void ToggleButton504_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!(bool)ToggleButton503.IsChecked)
            {
                ZwisePanel.Visibility = Visibility.Collapsed;
            }
        }

        private void ToggleButton503_Checked(object sender, RoutedEventArgs e)
        {
            ZwisePanel.Visibility = Visibility.Visible;
        }

        private void ToggleButton5221_Checked(object sender, RoutedEventArgs e)
        {
            ZAbsolute.Text = "Z-相对坐标";
            LambdaControl.Trigger("MUL_ZAXIS", this, new Dictionary<string, object>() { { "mode", 3 }, { "value", 1 }, { "zstart", windowData.MulDimensional.ZStart }, { "zend", windowData.MulDimensional.ZEnd } });
        }

        private void ToggleButton5221_Unchecked(object sender, RoutedEventArgs e)
        {
            ZAbsolute.Text = "Z-绝对坐标";
            LambdaControl.Trigger("MUL_ZAXIS", this, new Dictionary<string, object>() { { "mode", 3 }, { "value", 0 }, { "zstart", windowData.MulDimensional.ZStart }, { "zend", windowData.MulDimensional.ZEnd } });

        }
        private void ToggleButton222_Unchecked(object sender, RoutedEventArgs e)
        {
          windowData.MulDimensional.TInterval = "-1";
        }
        private static Dictionary<string, Timer> timers = new Dictionary<string, Timer>();
        //延时触发实现
        public static void AddEventTimer(string type, int millis, ElapsedEventHandler callback)
        {
            if (timers.TryGetValue(type, out var value))
            {
                value.Enabled = false;
                timers.Remove(type);
            }
            Timer timer = new Timer(millis);
            timer.Elapsed += callback;
            //timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Start();
            timers.Add(type, timer);
        }


        
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            
            //Application.Current.Dispatcher.Invoke(delegate
            //{
            //    WaitContorl.GetInstance().Hidden();
            //});

            Dictionary<string, object> data = new() { { "regularization", (double)windowData.OperatingMode.QuantitativePhase.Regularization * 0.01 } };
            LambdaControl.Trigger("QUANTITATIVE_PHASE_REG", Slider351, data);
           // MessageBox.Show(e.SignalTime.ToString());


        }



        private void Slider351_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AddEventTimer("351", 300, OnTimedEvent);

        }

        private void ToggleButton221_Unchecked(object sender, RoutedEventArgs e)
        {
          //  windowData.MulDimensional.TNumber = -1;
        }
    }


}


