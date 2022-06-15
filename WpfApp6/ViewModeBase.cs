﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp6
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