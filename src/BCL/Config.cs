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
    using Interfaces;
    using Newtonsoft.Json;

    public struct Config : IConfig {
        [JsonProperty("botToken")]
        public string BotToken { get; set; }
        [JsonProperty("prefix")]
        public char CommandPrefix { get; set; }
        [JsonProperty("logChannel")]
        public ulong LogChannel { get; set; }
    }
}