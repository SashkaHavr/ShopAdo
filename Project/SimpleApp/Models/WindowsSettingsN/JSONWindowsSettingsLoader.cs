using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApp.Models.WindowsSettingsN
{
    class JSONWindowsSettingsLoader : IWindowsSettingsLoader
    {
        public WindowSettings Load()
        {
            if (File.Exists("Settings.json"))
                return JsonConvert.DeserializeObject<WindowSettings>(File.ReadAllText("Settings.json"));
            return new WindowSettings();
        }
    }
}
