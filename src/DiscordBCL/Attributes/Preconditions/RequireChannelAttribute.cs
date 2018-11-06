using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBCL
{
    public class RequireChannelAttribute : PreconditionAttribute
    {
        private readonly ulong[] _channelIds;
        private readonly string[] _channelNames;

        public RequireChannelAttribute(params ulong[] channelIds)
            => _channelIds = channelIds;
        public RequireChannelAttribute(params string[] channelNames)
            => _channelNames = channelNames;

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            bool isValidChannel = false;

            if ((_channelIds != null && _channelIds.Contains(context.Channel.Id)) ||
                (_channelNames != null && _channelNames.Any(n => string.Equals(n, context.Channel.Name, StringComparison.OrdinalIgnoreCase))))
                isValidChannel = true;

            return isValidChannel
                  ? Task.FromResult(PreconditionResult.FromSuccess())
                  : Task.FromResult(PreconditionResult.FromError("This command cannot be run in this channel."));
        }
    }
}
