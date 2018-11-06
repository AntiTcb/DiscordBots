using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBCL
{
    public class RequireRoleAttribute : PreconditionAttribute
    {
        private readonly ulong[] _roleIds;

        public RequireRoleAttribute(params ulong[] roleIds)
            => _roleIds = roleIds;

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (!(context.User is IGuildUser guildUser))
                return Task.FromResult(PreconditionResult.FromError("This command must be used from within a guild."));

            if (guildUser.RoleIds.Intersect(_roleIds).Any())
                return Task.FromResult(PreconditionResult.FromSuccess());
            else
                return Task.FromResult(PreconditionResult.FromError("You do not have a role required to run this command."));            
        }
    }
}
