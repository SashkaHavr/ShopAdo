using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DAL;
using MaterialDesignThemes.Wpf;

namespace SimpleApp.Models.WindowsSettingsN
{
    class WindowSettings : NotifiedModelBase
    {
        PaletteHelper paletteHelper = new PaletteHelper();
        string dataSource;
        bool isDarkTheme = false;
        WindowState windowState = WindowState.Normal;
        

        public string DataSource { get => dataSource; set { dataSource = value; Notify(); } }
        public bool IsDarkTheme { get => isDarkTheme; set { isDarkTheme = value; Notify(); } }
        public WindowState WindowState { get => windowState; set { windowState = value; Notify(); } }
        public bool IsFullScreen
        {
            get
            {
                if (WindowState == WindowState.Maximized)
                    return true;
                return false;
            }
            set
            {
                if (value)
                    WindowState = WindowState.Maximized;
                else
                    WindowState = WindowState.Normal;
            }
        }

        public WindowSettings()
        {
            PropertyChanged += ToggleChanged;
        }
        
        void ToggleChanged(object o, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsDarkTheme")
                paletteHelper.SetLightDark(IsDarkTheme);
        }

        public void DragWindow(object o, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                (o as Window).DragMove();
        }

    }
}
