namespace BCL
{
    using Interfaces;
    using System.Collections.Generic;

    public static class Globals
    {
        public static IBotConfig BotConfig { get; set; }

        public static List<string> EvalImports { get; } = new List<string> {
            "Discord",
            "Discord.API",
            "Discord.Commands",
            "Discord.Rest",
            "Discord.WebSocket",
            "System",
            "System.Collections",
            "System.Collections.Generic",
            "System.Diagnostics",
            "System.IO",
            "System.Linq",
            "System.Math",
            "System.Reflection",
            "System.Runtime",
            "System.Threading.Tasks",
            "BCL"
        };

        public static Dictionary<ulong, ServerConfig> ServerConfigs { get; set; } = new Dictionary<ulong, ServerConfig>();
        public const string CONFIG_PATH = "config.json";
#if DEBUG
        public const string DEFAULT_PREFIX = ";>";
#else                                            
        public const string DEFAULT_PREFIX = ";";
#endif
        public const ulong OWNER_ID = 89613772372574208;
        public const string SERVER_CONFIG_PATH = "server_configs.json";
    }
}