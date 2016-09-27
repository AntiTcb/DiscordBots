#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/14/2016 9:51 PM
// Last Revised: 09/14/2016 9:51 PM
// Last Revised by: Alex Gravely
#endregion
namespace BCL {
    using System;
    using System.IO;
    using Newtonsoft.Json;

    public struct Config {
        [JsonProperty("botToken")]
        public string BotToken { get; set; }
        [JsonProperty("logChannel")]
        public ulong LogChannel { get; set; }
    }

    public static class ConfigHandler {
        public static Config Load(string path) {
            if (path == null) {
                throw new ArgumentNullException(nameof(path));
            }
            if (File.Exists(path)) {
                return JsonConvert.DeserializeObject<Config>
                    (File.ReadAllText(path));
            }
            var newConfig = new Config();
            Console.WriteLine("No config file detected! Please input config information now!");
            Console.WriteLine("Bot Token:");
            newConfig.BotToken = Console.ReadLine();

            Save(path, newConfig);
            return newConfig;
        }

        public static void Save(string path, Config config) => File.WriteAllText(path, JsonConvert.SerializeObject(config));
    }
}