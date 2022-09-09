using System;
using System.IO;
using Newtonsoft.Json;

namespace Jlaive
{
    internal class Settings
    {
        private static readonly string savepath = $"{AppDomain.CurrentDomain.BaseDirectory}\\bin";
        private static readonly string savefpath = savepath + "\\settings.json";

        public static SettingsObject Load() =>
            File.Exists(savefpath) ? JsonConvert.DeserializeObject<SettingsObject>(File.ReadAllText(savefpath)) : null;

        public static void Save(SettingsObject obj)
        {
            if (!Directory.Exists(savepath)) Directory.CreateDirectory(savepath);
            File.WriteAllText(savefpath, JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }

    internal class SettingsObject
    {
        public string InputFile { get; set; }
        public bool AntiDebug { get; set; }
        public bool AntiVM { get; set; }
        public bool SelfDelete { get; set; }
        public bool Hidden { get; set; }
        public bool Runas { get; set; }
        public bool ApiUnhook { get; set; }
        public string[] BindedFiles { get; set; }
    }
}
