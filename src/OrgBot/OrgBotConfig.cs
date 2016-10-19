#region Header
// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/18/2016 6:39 PM
// Last Revised: 10/18/2016 6:39 PM
// Last Revised by: Alex Gravely
#endregion
namespace OrgBot {
    using BCL.Interfaces;
    using Newtonsoft.Json;

    public struct OrgBotConfig : IBotConfig {
        #region Implementation of IBotConfig

        [JsonProperty("botToken")]
        public string BotToken { get; set; }
        [JsonProperty("logChannel")]
        public ulong LogChannel { get; set; }

        public OrgBotConfig(string botToken, ulong logChannel) {
            BotToken = botToken;
            LogChannel = logChannel;
        }

        #endregion
    }
}