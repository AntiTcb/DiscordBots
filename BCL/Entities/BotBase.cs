using BCL.Entities.Interfaces;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace BCL
{
    public abstract class BotBase<TBotConfig, TGuildConfig> : IBotBase<TBotConfig> 
        where TBotConfig : IBotConfig, new()
        where TGuildConfig : IGuildConfig, new() {

        public DiscordSocketClient Client { get; set ; }
        public TBotConfig Config { get; set; }

        public async Task InstallCommandsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task LoginAndConnectAsync(TokenType tokenType)
        {
            throw new NotImplementedException();
        }
    }
}
