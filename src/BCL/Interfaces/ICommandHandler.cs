#region Header

// Description:
//
// Solution: DiscordBots
// Project: BCL
//
// Created: 09/26/2016 11:16 PM
// Last Revised: 10/13/2016 7:54 PM
// Last Revised by: Alex Gravely

#endregion Header

namespace BCL.Interfaces
{
    #region Using

    using Discord.Commands;
    using Discord.WebSocket;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #endregion Using

    public interface ICommandHandler {

        #region Public Fields + Properties

        IBotConfig BotConfig { get; set; }
        DiscordSocketClient Client { get; set; }
        CommandService Service { get; set; }

        #endregion Public Fields + Properties

        #region Public Methods

        Task HandleCommandAsync(CommandContext ctx);

        Task InstallAsync
        (DiscordSocketClient c,
         IBotConfig botConfig,
         Dictionary<ulong, IServerConfig> serverConfigs,
         DependencyMap map = null);

        #endregion Public Methods
    }
}