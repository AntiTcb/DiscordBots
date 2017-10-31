using System.Collections.Generic;

namespace WiseOldBot
{
    using BCL.Interfaces;
    using Newtonsoft.Json;

    public struct WiseOldBotConfig : IBotConfig {
        [JsonProperty("botToken")]
        public string BotToken { get; set; }
        [JsonProperty("logChannel")]
        public ulong LogChannel { get; set; }
        [JsonProperty("usernameMap")]
        public Dictionary<ulong, string> UsernameMap { get; set; }
        [JsonProperty("getrackerToken")]
        public string GETrackerToken { get; set; }

        public WiseOldBotConfig(string botToken, ulong logChannel, string getrackerToken) {
            BotToken = botToken;
            LogChannel = logChannel;
            UsernameMap = new Dictionary<ulong, string>();
            GETrackerToken = getrackerToken;
        }
    }
}
