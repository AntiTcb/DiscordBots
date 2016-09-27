using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WiseOldBot
{
    using BCL.Interfaces;
    using Newtonsoft.Json;

    public struct WiseOldBotConfig : IConfig {
        [JsonProperty("botToken")]
        public string BotToken { get; set; }
        [JsonProperty("prefix")]
        public char CommandPrefix { get; set; }
        [JsonProperty("logChannel")]
        public ulong LogChannel { get; set; }
        [JsonProperty("usernameMap")]
        public Dictionary<ulong, string> UsernameMap { get; set; }
    }
}
