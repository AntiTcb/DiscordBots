#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/14/2016 11:08 PM
// Last Revised: 09/14/2016 11:17 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Preconditions {
    #region Using

    using System;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;

    #endregion

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireOwnerAttribute : PreconditionAttribute {
        #region Private Fields + Properties

        readonly IApplication _app;

        #endregion Private Fields + Properties

        #region Public Constructors

        public RequireOwnerAttribute(IApplication a) {
            _app = a;
        }

        #endregion Public Constructors

        #region Public Methods

        public override Task<PreconditionResult> CheckPermissions
            (IUserMessage context, Command executingCommand, object moduleInstance)
            =>
                Task.FromResult
                    (context.Author.Id == _app.Owner.Id
                         ? PreconditionResult.FromSuccess()
                         : PreconditionResult.FromError("You must be the owner of the bot."));

        #endregion Public Methods
    }
}