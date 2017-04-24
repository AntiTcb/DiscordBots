using Discord;              
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BCL.Models.Interfaces
{
    public interface IBotBase<T, U>
        where T : IBotConfig, new()
        where U : IGuildConfig, new()
    {
        DiscordSocketClient Client { get; set; }
        T Config { get; set; }
        Dictionary<ulong, U> GuildConfigs { get;}  

        Task InstallCommandsAsync();
        Task LoginAndConnectAsync(TokenType tokenType);
    }
}
