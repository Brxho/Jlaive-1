using System;
using System.IO;
using Newtonsoft.Json;

namespace Jlaive
{
    internal class Settings
    {
        private static readonly string savepath = AppDomain.CurrentDomain.BaseDirectory + "\\bin\\settings.json";

        public static SettingsObject Load() =>
            File.Exists(savepath) ? JsonConvert.DeserializeObject<SettingsObject>(File.ReadAllText(savepath)) : null;

        public static void Save(SettingsObject obj) => 
            File.WriteAllText(savepath, JsonConvert.SerializeObject(obj, Formatting.Indented));
    }

    internal class SettingsObject
    {
        public string inputFile { get; set; }
        public bool antiDebug { get; set; }
        public bool antiVM { get; set; }
        public bool selfDelete { get; set; }
        public bool hidden { get; set; }
        public bool runas { get; set; }
        public string[] bindedFiles { get; set; }
    }
}
