namespace BCL.Preconditions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord.Commands;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireUserAttribute : PreconditionAttribute
    {
        private readonly ulong[] _userIds;
        public RequireUserAttribute(params ulong[] userIds) 
            => _userIds = userIds;

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            bool isWhitelistedUser = _userIds.Contains(context.User.Id);
            return isWhitelistedUser
                ? Task.FromResult(PreconditionResult.FromSuccess())
                : Task.FromResult(PreconditionResult.FromError("You are not allowed to run this command."));
        }
    }
}
