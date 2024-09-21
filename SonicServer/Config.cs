using Newtonsoft.Json;

namespace SonicServer
{
    public class Settings
    {
        public string? DebugIP;
        public int? PortOverride;
        [JsonIgnore]
        public bool Developer = true;

        [JsonIgnore]
        public Theme? _Override_Theme = null;

        public string SelectedTheme = "DefaultDark";
        public Dictionary<string, Theme> CustomThemes = new Dictionary<string, Theme>();


        public void SaveToFile(string filename = "cfg")
        {
            Config.SaveToFile(this, filename);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class Config
    {
        static string FileDirectory = AppDomain.CurrentDomain.BaseDirectory; // shut up shut up shut up
        static string Resolve(string path)
        {
            return Path.Combine(FileDirectory, path);
        }

        public static void SaveToFile(Settings instance, string fileName = "cfg")
        {
            File.WriteAllText(Resolve($"{fileName}.json"), JsonConvert.SerializeObject(instance));
        }

        public static Settings ReadFromFile(string fileName = "cfg")
        {
            if (File.Exists(Resolve($"{fileName}.json")))
                return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Resolve($"{fileName}.json"))) ?? new Settings();
            else
                return new Settings();
        }
    }
}
