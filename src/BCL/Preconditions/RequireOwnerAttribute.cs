#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/26/2016 11:16 PM
// Last Revised: 10/25/2016 4:17 PM
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
        #region Overrides of PreconditionAttribute

        IApplication _appInfo;

        public async override Task<PreconditionResult> CheckPermissions(CommandContext ctx, CommandInfo cmd, IDependencyMap map) {
            if (_appInfo == null) {
                _appInfo = await ctx.Client.GetApplicationInfoAsync().ConfigureAwait(false);
            }
            return await Task.FromResult(
                ctx.User.Id == _appInfo.Owner.Id
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError("You are not the bot owner.")).ConfigureAwait(false);
        }

        #endregion Overrides of PreconditionAttribute
    }
}