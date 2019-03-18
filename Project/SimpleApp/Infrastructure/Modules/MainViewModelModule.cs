using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using SimpleApp.Models.WindowsSettingsN;

namespace SimpleApp.Infrastructure.Modules
{
    class MainViewModelModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IWindowsSettingsLoader>().To<JSONWindowsSettingsLoader>();
            Bind<IWindowsSettingsSaver>().To<JSONWindosSettingsSaver>();
        }
    }
}
