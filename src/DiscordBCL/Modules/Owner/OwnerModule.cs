using Discord;
using Discord.Commands;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBCL.Modules
{
    [Name("Owner"), Hidden]
    [RequireOwner]
    public partial class OwnerModule : ModuleBase<ShardedCommandContext>
    {
        private readonly CommandService _service;

        public OwnerModule(CommandService service)
            => _service = service;

        [Command("echo")]
        [Summary("Echoes the input string.")]
        [Remarks("echo potato")]
        public async Task EchoAsync([Remainder]string input)
            => await ReplyAsync(input).ConfigureAwait(false);

        [Command("set")]
        [Summary("Sets various properties about the bot.")]
        [Remarks("set game RuneScape")]
        public async Task SetAsync([Summary("Property to change.")] UserProperty property, [Summary("Desired value"), Remainder] string value = null)
        {
            switch (property)
            {
                case UserProperty.Username:
                    await Context.Client.CurrentUser.ModifyAsync(x => x.Username = value).ConfigureAwait(false);
                    break;
                case UserProperty.Nickname:
                    await Context.Guild.CurrentUser.ModifyAsync(x => x.Nickname = value).ConfigureAwait(false);
                    break;
                case UserProperty.Avatar:
                    using (var client = new HttpClient()) {
                        var imgStream = await client.GetStreamAsync(value).ConfigureAwait(false);
                        await Context.Client.CurrentUser.ModifyAsync(x => x.Avatar = new Image(imgStream));
                    }
                    break;
                case UserProperty.Game:
                    await Context.Client.SetGameAsync(value).ConfigureAwait(false);
                    break;
                case UserProperty.Status:
                    if (Enum.TryParse<UserStatus>(value, out var result))
                        await Context.Client.SetStatusAsync(result).ConfigureAwait(false);
                    break;
            }
            await ReplyAsync(":thumbsup:");
        }

        [Command("shutdown", RunMode = RunMode.Async)]
        [Alias("pd", "off")]
        [Summary("Terminates the application.")]
        [Remarks("shutdown")]
        public async Task ShutdownAsync()
        {
            await ReplyAsync("Shutting down.").ConfigureAwait(false);
            await Task.Delay(1500).ConfigureAwait(false);
            await Context.Client.StopAsync().ConfigureAwait(false);
            await Task.Delay(10000).ConfigureAwait(false);
            Environment.Exit(0);
        }

        public enum UserProperty
        {
            Username,
            Nickname,
            Nick = Nickname,
            Avatar,
            Avi = Avatar,
            Game,
            Status
        }
    }
}
