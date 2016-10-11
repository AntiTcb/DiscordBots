#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/27/2016 1:44 AM
// Last Revised: 10/10/2016 6:10 AM
// Last Revised by: Alex Gravely

#endregion

namespace BCL {
    #region Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Discord;
    using Interfaces;
    using Newtonsoft.Json;

    #endregion

    public static class ConfigHandler {
        #region Internal Structs + Classes

        internal static class ConfigBuilder {
            #region Internal Methods

            internal static T CreateBotConfig<T>() where T : IBotConfig, new() {
                object boxedConfig = new T();
                foreach (var prop in typeof(T).GetRuntimeProperties()) {
                    Console.WriteLine($"Input the value for property {prop.Name}:");
                    var propValue = Console.ReadLine();
                    prop.SetValue(boxedConfig, Convert.ChangeType(propValue, prop.PropertyType));
                }
                return (T) boxedConfig;
            }

            #endregion Internal Methods
        }

        #endregion Internal Structs + Classes

        #region Public Methods

        public static async Task<T> LoadBotConfigAsync<T>(string path = Globals.CONFIG_PATH) where T : IBotConfig, new() {
            if (File.Exists(path)) {
                return await Task.Run(() => JsonConvert.DeserializeObject<T>(File.ReadAllText(path))).ConfigureAwait(false);
            }
            Console.WriteLine("No config file detected.");
            var newConfig = ConfigBuilder.CreateBotConfig<T>();

            await SaveAsync(path, newConfig).ConfigureAwait(false);
            return newConfig;
        }

        public static async Task<Dictionary<ulong, IServerConfig>> LoadServerConfigsAsync
            (ITextChannel chan, string path = Globals.SERVER_CONFIG_PATH) {
            if (File.Exists(path)) {
                return await Task.Run(() =>
                                              JsonConvert.DeserializeObject<Dictionary<ulong, IServerConfig>>(File.ReadAllText(path))
                                     ).ConfigureAwait(false);
            }
            var newConfig = new Dictionary<ulong, IServerConfig>();
            await SaveAsync(path, newConfig).ConfigureAwait(false);
            return newConfig;
        }

        public static async Task SaveAsync(string path, Dictionary<ulong, IServerConfig> configs)
            => File.WriteAllText(path, await Task.Run(() => JsonConvert.SerializeObject(configs)).ConfigureAwait(false));

        public static async Task SaveAsync(string path, IBotConfig botConfig)
            => File.WriteAllText(path, await Task.Run(() => JsonConvert.SerializeObject(botConfig)).ConfigureAwait(false));

        #endregion Public Methods
    }
}