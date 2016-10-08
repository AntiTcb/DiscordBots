#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/27/2016 1:44 AM
// Last Revised: 10/05/2016 8:05 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL {
    #region Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Interfaces;
    using Newtonsoft.Json;

    #endregion

    public static class ConfigHandler {
        #region Public Methods

        // TODO: Refactor for IServerConfig
        public static async Task<T> LoadAsync<T>(string path) where T : Dictionary<ulong, IServerConfig>, IConfig, new() {
            if (path == null) {
                throw new ArgumentNullException(nameof(path));
            }
            if (File.Exists(path)) {
                return await Task.Run(() => JsonConvert.DeserializeObject<T>(File.ReadAllText(path)));
            }
            var newConfig = new T();


            Console.WriteLine("No config file detected. Input config information.\nBot Token:");
            newConfig.BotToken = Console.ReadLine();

            await SaveAsync(path, newConfig);
            return newConfig;
        }

        public static async Task SaveAsync<T>(string path, T configs) where T : Dictionary<ulong, IServerConfig>, IConfig
            => File.WriteAllText(path, await Task.Run(() => JsonConvert.SerializeObject(configs)));

        public static async Task SaveAsync(string path, IConfig config)
            => File.WriteAllText(path, await Task.Run(() => JsonConvert.SerializeObject(config)));

        #endregion Public Methods
    }
}