

namespace BCL.Preconditions
{
    using Discord.Commands;
    using System.Threading.Tasks;
    using Discord;

    public class RequireUserPermissionOrOwnerAttribute : RequireUserPermissionAttribute
    {
        public RequireUserPermissionOrOwnerAttribute(GuildPermission permission) : base(permission) { }
        public RequireUserPermissionOrOwnerAttribute(ChannelPermission permission) : base(permission) { }

        public override async Task<PreconditionResult> CheckPermissions(CommandContext context, CommandInfo command, IDependencyMap map)
        {
            var guildUser = context.User as IGuildUser;
            var appInfo = await context.Client.GetApplicationInfoAsync();

            if (appInfo.Owner.Id == context.User.Id)
                return PreconditionResult.FromSuccess();

            if (GuildPermission.HasValue)
            {
                if (guildUser == null)
                    return PreconditionResult.FromError("Command must be used in a guild channel");
                if (!guildUser.GuildPermissions.Has(GuildPermission.Value))
                    return PreconditionResult.FromError($"Command requires guild permission {GuildPermission.Value}");
            }

            if (ChannelPermission.HasValue)
            {
                var guildChannel = context.Channel as IGuildChannel;

                ChannelPermissions perms;
                if (guildChannel != null)
                    perms = guildUser.GetPermissions(guildChannel);
                else
                    perms = ChannelPermissions.All(guildChannel);

                if (!perms.Has(ChannelPermission.Value))
                    return PreconditionResult.FromError($"Command requires channel permission {ChannelPermission.Value}");
            }

            return PreconditionResult.FromSuccess();
        }
    }
}

