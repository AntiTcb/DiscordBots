#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/09/2016 9:23 PM
// Last Revised: 10/09/2016 9:23 PM
// Last Revised by: Alex Gravely
#endregion
namespace BCL {
    using System.Collections.Generic;
    using Interfaces;

    public static class Globals {
        public const string CONFIG_PATH = "config.json";
        public const string SERVER_CONFIG_PATH = "server_configs.json";
        public const ulong OWNER_ID = 89613772372574208;
        public const string DEFAULT_PREFIX = ";";

        public static IBotConfig BotConfig { get; set; }
        public static Dictionary<ulong, ServerConfig> ServerConfigs { get; set; } = new Dictionary<ulong, ServerConfig>();

        public static List<string> EvalImports { get; } = new List<string> {
            "Discord",
            "Discord.API",
            "Discord.Commands",
            "Discord.WebSocket",
            "System",
            "System.Collections",
            "System.Collections.Generic",
            "System.Linq",
            "System.Math",
            "System.Reflection",
            "System.Runtime",
            "System.Threading.Tasks"
        };
    }
}