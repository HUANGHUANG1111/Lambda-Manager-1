﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

//预备拆分出来的代码
namespace LambdaManager.Core
{
    /// <summary>
    /// 实例化一个Mode
    /// </summary>
    public abstract class ViewModeBase :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 触发消息通知事件
        /// </summary>
        /// <param name="propertyName"></param>
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
