using System.IO;
using Newtonsoft.Json;

namespace DiscordBCL.Configuration
{
    public abstract class ConfigBase
    {
        [JsonIgnore]
        public static string FileName { get; protected set; } = "config.json";

        public ConfigBase(string fileName) 
            => FileName = fileName;

        public void SaveJson()
        {
            string file = Path.Combine("configs", FileName);
            File.WriteAllText(file, ToJson());
        }

        public static T Load<T>() where T : ConfigBase
        {
            string file = Path.Combine("configs", FileName);
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(file));
        }

        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
