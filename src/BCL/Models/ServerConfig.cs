#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/13/2016 7:56 PM
// Last Revised: 11/06/2016 2:15 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL {
    #region Using

    using System.Collections.Generic;
    using Interfaces;
    using Newtonsoft.Json;

    #endregion

    public struct ServerConfig : IServerConfig {
        #region Public Fields + Properties

        public const string DefaultPrefix = ";";

        #endregion Public Fields + Properties

        #region Implementation of IServerConfig

        [JsonProperty("commandPrefix")]
        public string CommandPrefix { get; set; }

        [JsonProperty("tags")]
        public Dictionary<string, string> Tags { get; set; }

        #endregion Implementation of IServerConfig

        #region Public Constructors

        public ServerConfig(string commandPrefix, Dictionary<string, string> tags) {
            CommandPrefix = commandPrefix;
            Tags = tags;
        }

        #endregion Public Constructors
    }
}