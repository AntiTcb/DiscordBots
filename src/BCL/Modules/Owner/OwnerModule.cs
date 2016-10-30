#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/16/2016 5:11 PM
// Last Revised: 10/27/2016 11:13 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Modules.Owner {
    #region Using

    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Discord;
    using Discord.API;
    using Discord.Commands;
    using Discord.WebSocket;
    using Extensions;
    using Interfaces;
    using Preconditions;

    #endregion

    [RequireOwner]
    public partial class OwnerModule : ModuleBase {
        #region Public Methods

        [Command("powerdown"), Alias("pd")]
        public async Task PowerdownAsync() {
            await ReplyAsync("Powering down!").ConfigureAwait(false);
            await Context.Client.DisconnectAsync().ConfigureAwait(false);
            await Task.Delay(1500).ConfigureAwait(false);
            Environment.FailFast("");
        }

        [Command("echo")]
        public async Task EchoAsync([Remainder] string text) {
            await ReplyAsync(text).ConfigureAwait(false);
        }

        [Command("set"), RequireContext(ContextType.Guild)]
        public async Task SetAsync(UserProperty prop, [Remainder] string value) {
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
                                Context.Client.CurrentUser.ModifyAsync(x => x.Avatar = new Image(imagestream)).
                                        ConfigureAwait(false);
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
                        (Context.Client as DiscordSocketClient).SetStatus((UserStatus) newStatus).ConfigureAwait(false);
                    break;

                default:
                    await ReplyAsync($"**ERROR**: {nameof(prop)}, {prop}").ConfigureAwait(false);
                    return;
            }
            await ReplyAsync(":thumbsup:").ConfigureAwait(false);
        }

        #endregion Public Methods
    }
}