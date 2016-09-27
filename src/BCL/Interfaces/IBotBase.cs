#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/14/2016 4:10 AM
// Last Revised: 09/14/2016 3:59 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Interfaces {
    using System.Threading.Tasks;
    using Discord.WebSocket;

    public interface IBotBase {
        string BotToken { get; set; }
        DiscordSocketClient Client { get; set; }
        ICommandHandler Commands { get; set; }
        IConfig Configs { get; set; }
        Task StartAsync();
        void HandleConfigs();
        Task InstallCommandsAsync();
        Task LoginAndConnectAsync();
    }
}