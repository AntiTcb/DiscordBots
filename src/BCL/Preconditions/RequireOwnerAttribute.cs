#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/26/2016 11:16 PM
// Last Revised: 10/13/2016 7:32 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Preconditions {
    #region Using

    using System;
    using System.Threading.Tasks;
    using Discord.Commands;

    #endregion

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireOwnerAttribute : PreconditionAttribute {
        #region Overrides of PreconditionAttribute

        public override Task<PreconditionResult> CheckPermissions
            (CommandContext context, CommandInfo command, IDependencyMap map) {
            return Task.FromResult
                (context.User.Id == Globals.OWNER_ID
                     ? PreconditionResult.FromSuccess() : PreconditionResult.FromError("You are not the bot owner."));
        }

        #endregion Overrides of PreconditionAttribute
    }
}