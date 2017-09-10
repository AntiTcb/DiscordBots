namespace BCL.Preconditions
{
    using Discord.Commands;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireChannelAttribute : PreconditionAttribute
    {
        ulong[] _channelIds;

        public RequireChannelAttribute(params ulong[] channelIds)
        {
            _channelIds = channelIds;
        }

        public override async Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var isWhitelistedChannel = _channelIds.Contains(context.Channel.Id);
            return await Task.FromResult(isWhitelistedChannel)
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError("This command is not allowed in this channel.");
        }
    }
}