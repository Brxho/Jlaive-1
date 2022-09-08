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
        public string inputFile { get; set; }
        public bool antiDebug { get; set; }
        public bool antiVM { get; set; }
        public bool selfDelete { get; set; }
        public bool hidden { get; set; }
        public bool runas { get; set; }
        public bool apiUnhook { get; set; }
        public string[] bindedFiles { get; set; }
    }
}
