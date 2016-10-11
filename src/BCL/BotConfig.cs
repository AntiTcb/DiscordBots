#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/14/2016 9:51 PM
// Last Revised: 10/10/2016 5:08 AM
// Last Revised by: Alex Gravely

#endregion

namespace BCL {
    #region Using

    using System;
    using System.Reflection;
    using Interfaces;
    using Newtonsoft.Json;

    #endregion

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