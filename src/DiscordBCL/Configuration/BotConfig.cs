using Newtonsoft.Json;
using System;
using System.IO;

namespace DiscordBCL.Configuration
{
    public class BotConfig : ConfigBase
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("logChannelId")]
        public ulong LogChannelId { get; set; }

        public BotConfig() : base("bot_config.json") { }
        static BotConfig() => FileName = "bot_config.json";

        public static BotConfig Load()
        {
            EnsureExists(); 
            return Load<BotConfig>();
        }
        
        internal static void EnsureExists()
        {
            string file = Path.Combine(AppContext.BaseDirectory, "configs", FileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var config = new BotConfig();
                config.SaveJson();
            }
        }
    }
}
