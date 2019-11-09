namespace BCL.Modules.Owner
{
    using Discord;
    using Discord.Commands;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    [Name("Owner"), RequireOwner]
    public partial class OwnerModule : ModuleBase<SocketCommandContext> {

        [Command("echo"), Summary("Echos the user input."), Remarks("echo potato")]
        public async Task EchoAsync([Remainder, Summary("Text to echo")] string text) 
            => await ReplyAsync(text).ConfigureAwait(false);

        [Command("powerdown", RunMode = RunMode.Async), Alias("pd"), Summary("Terminates the bot application"), Remarks("powerdown")]
        public async Task PowerdownAsync() {
            await ReplyAsync("Powering down!").ConfigureAwait(false);
            await Task.Delay(1500).ConfigureAwait(false);
            await Context.Client.StopAsync().ConfigureAwait(false);
            await Task.Delay(1500).ConfigureAwait(false);
        }

        [Command("getinvite"), Summary("Makes an invite to the specified guild"), Remarks("getinvite 123456")]
        public async Task GetInviteAsync([Summary("Target guild")]ulong guild)
        {
            var g = Context.Client.GetGuild(guild);
            if (g.CurrentUser.GetPermissions(g.DefaultChannel).CreateInstantInvite)
            {
                var invite = await Context.Client.GetGuild(guild).DefaultChannel.CreateInviteAsync();
                await ReplyAsync(invite.Url);
                return;
            }
            await ReplyAsync("I lack Create Instant Invite permissions in that guild.");
        }

        [Command("set"), RequireContext(ContextType.Guild), Summary("sets various bot properties"), Remarks("set nick FooBar")]
        public async Task SetAsync([Summary("Property to change")]UserProperty prop,
            [Summary("Value to change the property to"), Remainder] string value) {
            switch (prop) {
                case UserProperty.User:
                    await Context.Client.CurrentUser.ModifyAsync(x => x.Username = value).ConfigureAwait(false);
                    break;

                case UserProperty.Nick:
                    await Context.Guild.CurrentUser.ModifyAsync(x => x.Nickname = value).ConfigureAwait(false);
                    break;

                case UserProperty.Avatar:
                string url = value;
                    if (value == "reset") {
                        var app = await Context.Client.GetApplicationInfoAsync().ConfigureAwait(false);
                        url = app.IconUrl;
                    }
                    Uri q;
                    Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out q);

                    if (q == null) return;
                    using (var client = new HttpClient()) {
                        using (var imagestream = await client.GetStreamAsync(q)) {
                            await Context.Client.CurrentUser.ModifyAsync(x => x.Avatar = new Image(imagestream)).ConfigureAwait(false);
                        }
                    }
                    break;

                case UserProperty.Game:
                    await Context.Client.SetGameAsync(value).ConfigureAwait(false);
                    break;

                case UserProperty.Status:
                    UserStatus st;
                    if (Enum.TryParse<UserStatus>(value, out st))
                        await Context.Client.SetStatusAsync(st).ConfigureAwait(false);
                    break;

                default:
                    await ReplyAsync($"**ERROR**: {nameof(prop)}, {prop}").ConfigureAwait(false);
                    return;
            }
            await ReplyAsync(":thumbsup:").ConfigureAwait(false);
        }
    }
}