#region Header
// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/19/2016 10:47 PM
// Last Revised: 10/19/2016 10:47 PM
// Last Revised by: Alex Gravely
#endregion
namespace OrgBot.Preconditions {
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Discord.WebSocket;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireRole : PreconditionAttribute {
        #region Overrides of PreconditionAttribute

        ulong _roleId;

        public RequireRole(ulong roleId) {
            _roleId = roleId;
        }

        public override Task<PreconditionResult> CheckPermissions(CommandContext context, CommandInfo command, IDependencyMap map) {
            var guildUser = context.User as SocketGuildUser;
            return Task.FromResult(guildUser.RoleIds.Contains(_roleId)
                       ? PreconditionResult.FromSuccess()
                       : PreconditionResult.FromError("You do not have the role required for this command."));
        }

        #endregion
    }
}