namespace BCL {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Interfaces;
    using Newtonsoft.Json;

    public static partial class ConfigHandler {

        public static async Task<T> LoadBotConfigAsync<T>(string path = Globals.CONFIG_PATH) where T : IBotConfig, new() {
            if (File.Exists(path))
                return await Task.Run(() => JsonConvert.DeserializeObject<T>(File.ReadAllText(path))).ConfigureAwait(false);

            Console.WriteLine("No config file detected.");
            var newConfig = ConfigBuilder.CreateBotConfig<T>();

            await SaveAsync(path, newConfig).ConfigureAwait(false);
            return newConfig;
        }

        public static async Task<Dictionary<ulong, T>> LoadServerConfigsAsync<T>(string path = Globals.SERVER_CONFIG_PATH) 
            where T : IServerConfig, new() {
            if (File.Exists(path)) 
                return await Task.Run(() => JsonConvert.DeserializeObject<Dictionary<ulong, T>>(File.ReadAllText(path))).ConfigureAwait(false);

            var newConfig = new Dictionary<ulong, T>();
            await SaveAsync(path, newConfig).ConfigureAwait(false);
            return newConfig;
        }

        public static async Task SaveAsync<T>(string path, Dictionary<ulong, T> configs) where T : IServerConfig
        => File.WriteAllText(path, await Task.Run(() => JsonConvert.SerializeObject(configs, Formatting.Indented)).ConfigureAwait(false));

        public static async Task SaveAsync(string path, IBotConfig botConfig)
            => File.WriteAllText(path, await Task.Run(() => JsonConvert.SerializeObject(botConfig, Formatting.Indented)).ConfigureAwait(false));

    }
}