using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SimpleApp.Models.WindowsSettingsN
{
    class JSONWindosSettingsSaver : IWindowsSettingsSaver
    {
        public void Save(WindowSettings windowSettings)
        {
            File.WriteAllText("Settings.json", JsonConvert.SerializeObject(windowSettings));
        }
    }
}
