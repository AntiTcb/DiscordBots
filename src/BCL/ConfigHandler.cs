#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/27/2016 1:44 AM
// Last Revised: 09/27/2016 1:44 AM
// Last Revised by: Alex Gravely
#endregion
namespace BCL {
    using System;
    using System.IO;
    using Interfaces;
    using Newtonsoft.Json;

    public static class ConfigHandler
    {
        public static IConfig Load<T>(string path) where T : IConfig, new()
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<T>
                    (File.ReadAllText(path));
            }
            var newConfig = new T();
            Console.WriteLine("No config file detected! Please input config information now!");
            Console.WriteLine("Bot Token:");
            newConfig.BotToken = Console.ReadLine();

            Save(path, newConfig);
            return newConfig;
        }

        public static void Save(string path, IConfig config) => File.WriteAllText(path, JsonConvert.SerializeObject(config));
    }
}