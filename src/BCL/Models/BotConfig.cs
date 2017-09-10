namespace BCL {

    using Interfaces;
    using Newtonsoft.Json;

    public struct BotConfig : IBotConfig {

        [JsonProperty("botToken")]
        public string BotToken { get; set; }

        [JsonProperty("logChannel")]
        public ulong LogChannel { get; set; }

        public BotConfig(string botToken, ulong logChannel) {
            BotToken = botToken;
            LogChannel = logChannel;
        }
    }
}