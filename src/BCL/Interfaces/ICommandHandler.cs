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

        DiscordSocketClient Client { get; set; }
        CommandService Service { get; set; }
        IDependencyMap Map { get; set; }

        #endregion Public Fields + Properties

        #region Public Methods

        Task HandleCommandAsync(SocketMessage msg);

        Task InstallAsync(IDependencyMap map = null);

        #endregion Public Methods
    }
}