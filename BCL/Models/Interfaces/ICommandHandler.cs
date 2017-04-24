using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace BCL.Models.Interfaces
{
    public interface ICommandHandler
    {
        IDiscordClient Client { get; }
        CommandService Service { get; }
        IDependencyMap Map { get; }

        Task HandleCommandAsync(SocketMessage msg);
        Task InstallAsync();
    }
}
