using Discord;              
using Discord.WebSocket;    
using System.Threading.Tasks;

namespace BCL.Entities.Interfaces
{
    public interface IBotBase<T> where T : IBotConfig, new()
    {
        DiscordSocketClient Client { get; set; }
        T Config { get; set; }
        //ICommandHandler Commands { get; set; }

        Task InstallCommandsAsync();
        Task LoginAndConnectAsync(TokenType tokenType);
    }
}
