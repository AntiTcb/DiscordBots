namespace BCL.Preconditions
{
    using Discord;
    using Discord.Commands;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireRoleAttribute : PreconditionAttribute
    {
        ulong[] _roleIds;

        public RequireRoleAttribute(params ulong[] roleIds)
        {
            _roleIds = roleIds;
        }

        public override async Task<PreconditionResult> CheckPermissions(CommandContext context, CommandInfo command, IDependencyMap map)
        {
            if (!(context.User is IGuildUser))
            {
                return PreconditionResult.FromError("This command may only be run within a guild.");
            }
            var guildUser = context.User as IGuildUser;
            var hasRole = guildUser.RoleIds.Intersect(_roleIds).Any();
            return await Task.FromResult(hasRole)
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError("You do not have a role required for this command.");
        }
    }
}