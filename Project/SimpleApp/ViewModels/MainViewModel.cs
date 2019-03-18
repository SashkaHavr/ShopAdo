using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DAL;
using MaterialDesignThemes.Wpf;
using SimpleApp.Infrastructure;
using SimpleApp.Models;
using SimpleApp.Models.WindowsSettingsN;
using SimpleApp.Views;
using SimpleApp.Views.Controls;

namespace SimpleApp.ViewModels
{
    class MainViewModel : NotifiedModelBase
    {
        UserControl saleView = new SaleView();
        UserControl editView = new EditView();
        UserControl currentView;
        bool isSale;
        IWindowsSettingsSaver settingsSaver;
        ICommand fullScreenCommand;
        ICommand closeCommand;
        ICommand closingCommand;

        public WindowSettings Settings { get; }
        public UserControl CurrentView { get => currentView; set { currentView = value; Notify(); }  }
        public bool IsSale { get => isSale; set { isSale = value; Notify(); } }

        public ICommand CloseCommand { get => closeCommand ?? (closeCommand = new RelayCommand(o => (o as Window).Close())); }
        public ICommand FullScreenCommand { get => fullScreenCommand ?? (fullScreenCommand = new RelayCommand(IsFullScreenChange)); }
        public ICommand ClosingCommand { get => closingCommand ?? (closingCommand = new RelayCommand(o => settingsSaver.Save(Settings))); }

        public MainViewModel(IWindowsSettingsLoader settingsLoader, IWindowsSettingsSaver settingsSaver)
        {
            PropertyChanged += ToggleChanged;
            IsSale = true;
            Settings = settingsLoader.Load();
            this.settingsSaver = settingsSaver;
        }
        
        void IsFullScreenChange(object o)
        {
            if (Settings.IsFullScreen)
                Settings.IsFullScreen = false;
            else
                Settings.IsFullScreen = true;
        }

        void ToggleChanged(object o, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSale")
                if (isSale)
                    CurrentView = saleView;
                else
                    CurrentView = editView;
        }

    }
}
