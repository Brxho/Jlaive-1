using System;
using System.IO;
using Newtonsoft.Json;

namespace Jlaive
{
    internal class Settings
    {
        private static readonly string _savePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\bin";
        private static readonly string _savefPath = _savePath + "\\settings.json";

        public static SettingsObject Load() =>
            File.Exists(_savefPath) ? JsonConvert.DeserializeObject<SettingsObject>(File.ReadAllText(_savefPath)) : null;

        public static void Save(SettingsObject obj)
        {
            if (!Directory.Exists(_savePath)) Directory.CreateDirectory(_savePath);
            File.WriteAllText(_savefPath, JsonConvert.SerializeObject(obj, Formatting.Indented));
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
