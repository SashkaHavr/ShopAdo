using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApp.Models.WindowsSettingsN
{
    interface IWindowsSettingsSaver 
    {
        void Save(WindowSettings windowSettings);
    }
}
