namespace BCL {

    using System.Collections.Generic;
    using Interfaces;
    using Newtonsoft.Json;

    public struct ServerConfig : IServerConfig {

#if DEBUG
        public const string DefaultPrefix = "debug>>";
#else
        public const string DefaultPrefix = ";";
#endif
        [JsonProperty("commandPrefix")]
        public string CommandPrefix { get; set; }

        [JsonProperty("tags")]
        public Dictionary<string, string> Tags { get; set; }

        public ServerConfig(string commandPrefix, Dictionary<string, string> tags) {
            CommandPrefix = commandPrefix;
            Tags = tags;
        }

    }
}