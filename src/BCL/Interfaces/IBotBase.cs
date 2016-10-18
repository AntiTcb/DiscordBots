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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;

    public interface IBotBase {
        DiscordSocketClient Client { get; set; }
        ICommandHandler Commands { get; set; }

        Task StartAsync<T>() where T : IBotConfig, new();
        Task HandleConfigsAsync<T>() where T : IBotConfig, new();
        Task InstallCommandsAsync();
        Task LoginAndConnectAsync(TokenType tokenType);
    }
}