namespace BCL.Preconditions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord.Commands;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireUserAttribute : PreconditionAttribute
    {
        ulong[] _userIds;
        public RequireUserAttribute(params ulong[] userIds)
        {
            _userIds = userIds;
        }

        public override async Task<PreconditionResult> CheckPermissions(CommandContext context, CommandInfo command, IDependencyMap map)
        {
            var isWhitelistedUser = _userIds.Contains(context.User.Id);
            return await Task.FromResult(isWhitelistedUser)
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError("You are not allowed to run this command.");
        }
    }
}
