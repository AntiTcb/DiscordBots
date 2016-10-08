#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/14/2016 4:19 PM
// Last Revised: 09/14/2016 4:19 PM
// Last Revised by: Alex Gravely
#endregion
namespace BCL.Interfaces {
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;

    public interface ICommandHandler {
        CommandService Service { get; set; }
        DiscordSocketClient Client { get; set; }
        ISelfUser Self { get; set; }
        IConfig Config { get; set; }
        Task Install(DiscordSocketClient c, IConfig config, DependencyMap map = null);
        Task HandleCommand(IMessage paramMessage);
    }
}