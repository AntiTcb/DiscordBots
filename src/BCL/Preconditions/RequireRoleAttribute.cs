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
        private readonly ulong[] _roleIds;

        public RequireRoleAttribute(params ulong[] roleIds)
        {
            _roleIds = roleIds;
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.Guild == null)
                return Task.FromResult(PreconditionResult.FromError("This command may only be run within a guild."));

            var guildUser = context.User as IGuildUser;
            var hasRole = guildUser.RoleIds.Intersect(_roleIds).Any();
            return hasRole
                ? Task.FromResult(PreconditionResult.FromSuccess())
                : Task.FromResult(PreconditionResult.FromError("You do not have a role required for this command."));
        }
    }
}