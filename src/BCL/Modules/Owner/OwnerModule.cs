// Description:
//
// Solution: DiscordBots
// Project: BCL
//
// Created: 10/16/2016 5:11 PM
// Last Revised: 11/04/2016 12:52 AM
// Last Revised by: Alex Gravely

namespace BCL.Modules.Owner {

    using Discord;
    using Discord.API;
    using Discord.Commands;
    using Discord.WebSocket;
    using Extensions;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    [Name("Owner"), RequireOwner]
    public partial class OwnerModule : ModuleBase {

        [Command("echo"), Summary("Echos the user input."), Remarks("echo potato")]
        public async Task EchoAsync([Remainder, Summary("Text to echo")] string text) {
            await ReplyAsync(text).ConfigureAwait(false);
        }

        [Command("powerdown"), Alias("pd"), Summary("Terminates the bot application"), Remarks("powerdown")]
        public async Task PowerdownAsync() {
            await ReplyAsync("Powering down!").ConfigureAwait(false);
            await Context.Client.DisconnectAsync().ConfigureAwait(false);
            await Task.Delay(1500).ConfigureAwait(false);
        }

        [Command("say"), Summary("Echos user input into the specified channel"), Remarks("say #general potato")]
        public async Task SayAsync([Summary("Target channel")] ITextChannel channel, [Remainder, Summary("Text to echo")] string text) {
            (await Context.Client.GetChannelAsync(channel.Id) as SocketTextChannel)?.SendMessageAsync(text);
        }

        //[Command("getinvite"), Summary("Makes an invite to the specified guild"), Remarks("getinvite 123456")]
        //public async Task GetInviteAsync([Summary("Target guild")]IGuild guild)
        //{
        //    var channel = await Context.Client.GetChannelAsync(guild.DefaultChannelId);
        //    var invite = await (channel as SocketGuildChannel).CreateInviteAsync();
        //    await ReplyAsync(invite.Url);
        //}

        [Command("set"), RequireContext(ContextType.Guild), Summary("sets various bot properties"), Remarks("set nick FooBar")]
        public async Task SetAsync([Summary("Property to change")]UserProperty prop,
            [Summary("Value to change the property to"), Remainder] string value) {
            switch (prop) {
                case UserProperty.User:
                    await Context.Client.CurrentUser.ModifyAsync(x => x.Username = value).ConfigureAwait(false);
                    break;

                case UserProperty.Nick:
                    await
                        (await Context.Guild.GetCurrentUserAsync().ConfigureAwait(false)).ModifyAsync
                            (x => x.Nickname = value).ConfigureAwait(false);
                    break;

                case UserProperty.Avatar:
                    var url = value;
                    if (value == "reset") {
                        var app = await Context.Client.GetApplicationInfoAsync().ConfigureAwait(false);
                        url = app.IconUrl;
                    }
                    var q = new Uri(url);
                    using (var client = new HttpClient()) {
                        await client.DownloadAsync(q, q.LocalPath.Replace("/", "")).ConfigureAwait(false);
                        using (var imagestream = new FileStream(q.LocalPath.Replace("/", ""), FileMode.Open)) {
                            await
                                Context.Client.CurrentUser.ModifyAsync(x => x.Avatar = new Image(imagestream)).ConfigureAwait(false);
                        }
                        File.Delete(q.LocalPath.Replace("/", ""));
                    }
                    break;

                case UserProperty.Game:
                    await (Context.Client as DiscordSocketClient).SetGame(value).ConfigureAwait(false);
                    break;

                case UserProperty.Status:
                    var newStatus = Enum.Parse(typeof(UserStatus), value);
                    await
                        (Context.Client as DiscordSocketClient).SetStatus((UserStatus)newStatus).ConfigureAwait(false);
                    break;

                default:
                    await ReplyAsync($"**ERROR**: {nameof(prop)}, {prop}").ConfigureAwait(false);
                    return;
            }
            await ReplyAsync(":thumbsup:").ConfigureAwait(false);
        }
    }
}