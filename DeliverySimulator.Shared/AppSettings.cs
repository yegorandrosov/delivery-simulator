using DeliverySimulator.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Shared
{
    public class AppSettings
    {
        public AppConfiguration AppConfig { get; }
        private AppSettings()
        {
            var appSettingsContent = File.ReadAllText("appsettings.json");
            AppConfig = JsonConvert.DeserializeObject<AppConfiguration>(appSettingsContent);
        }

        public static readonly AppSettings Instance;

        static AppSettings()
        {
            Instance = new AppSettings();
        }
    }
}
