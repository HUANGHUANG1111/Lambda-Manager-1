using Global.Common.Converter.Json;
using Global.Common.Extensions;
using Global.Hardware;
using Global.Mode;
using Global.Mode.Config;
using Mode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Global
{

    public partial class WindowData
    {
        private static WindowData _instance;
        private static readonly object _locker = new();

        public static WindowData GetInstance()
        {
            lock (_locker) { _instance ??= new WindowData(); }
            return _instance;
        }

        public DeviceInformation deviceInformation = new DeviceInformation();
        public ExposureViewMode ExposureViewMode = new ExposureViewMode();

        public MapModel mapModel = new MapModel();

        private WindowData()
        {
            Common.Config.ConfigReadEvent += ReadConfig;
            Common.Config.ConfigSetEvent += SetValue;
            Common.Config.ConfigWriteEvent += SaveConfig;
            Interference();
            Hardware_Initialized();
            AddEventHandler();
            AddInjection();
            AddInjection1();
        }

        public string FilePath;

        public MulDimensional MulDimensional = new();

        public Stage Stage = new() {};

        public OperatingMode OperatingMode = new();
        

        public Config Config = new();


        public void SetValue()
        {
            if (!ACQUIRE)
            {
                MulDimensional.ZStart = Config.Dimensional.ZstackWiseSerial.ZBegin;
                MulDimensional.Zstep = Config.Dimensional.ZstackWiseSerial.ZStep;
                MulDimensional.ZEnd = Config.Dimensional.ZstackWiseSerial.ZEnd;

                OperatingMode.SetValue(Config.OperatingMode);
                Stage.SetValue(Config.Stage);
                ImageViewState.SetValue(Config.ImageViewState);
                Update.UpdateGlobal();
            }
            else
            {
                MessageBox.Show("请先暂停采集模式，在对参数进行赋值");
            }

        }
        public ImageViewState ImageViewState = new ImageViewState();


        public void SaveConfig(string ConfigFullName)
        {
            Config.Dimensional.ZstackWiseSerial.ZBegin = MulDimensional.ZStart;
            Config.Dimensional.ZstackWiseSerial.ZStep = MulDimensional.Zstep;
            Config.Dimensional.ZstackWiseSerial.ZEnd = MulDimensional.ZEnd;
            Config.OperatingMode.SetValue(OperatingMode);
            Config.Stage.SetValue(Stage);
            Config.ImageViewState.SetValue(ImageViewState);

            Config.LastOpenTime = DateTime.Now.ToString();
            Config.Dimensional.Saveprefix = ConfigFullName;
            Config.ToJsonFile(ConfigFullName);
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns></returns>
        public int ReadConfig(string ConfigFullName)
        {
            if (!File.Exists(ConfigFullName))
            {
                MessageBox.Show("找不到工程文件。");
                return -1;
            }
            string result = File.ReadAllText(ConfigFullName);
            if (result==null)
            {
                MessageBox.Show("未能加载项目文件。缺少根元素");
                return -2;
            }

            try
            {
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                jsonSerializerOptions.Converters.Add(new SolidColorBrushConverter());
                Config = JsonSerializer.Deserialize<Config>(result, jsonSerializerOptions);
            }
            catch (Exception ex)
            {
                MessageBox.Show("未能加载项目文件。" + ex.Message);
                return -3;
            }
            FilePath = ConfigFullName;
            return 0;
        }
    }
}
