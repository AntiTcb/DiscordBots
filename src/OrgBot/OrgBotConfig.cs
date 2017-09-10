namespace OrgBot {
    using BCL.Interfaces;
    using Newtonsoft.Json;

    public struct OrgBotConfig : IBotConfig {

        [JsonProperty("botToken")]
        public string BotToken { get; set; }
        [JsonProperty("logChannel")]
        public ulong LogChannel { get; set; }
        [JsonProperty("dbToken")]
        public string DatabaseToken { get; set; }     

        public OrgBotConfig(string botToken, ulong logChannel, string dbToken) {
            BotToken = botToken;
            LogChannel = logChannel;
            DatabaseToken = dbToken;
        }

    }
}