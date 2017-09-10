using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace BCL
{
    public class CommandHandler {

        public DiscordSocketClient Client { get; set; }
        public IServiceProvider Services { get; set; }
        public CommandService CommandService { get; set; }

        public CommandHandler(DiscordSocketClient client, IServiceProvider provider, CommandService cmdService)
        {
            Client = client;
            Services = provider;
            CommandService = cmdService;
        }

        public async virtual Task HandleCommandAsync(SocketMessage msg) {
            var message = msg as SocketUserMessage;
            if (message == null || msg.Author.Id == Client.CurrentUser.Id) 
                return;

            var argPos = 0;
            var guildChannel = message.Channel as SocketGuildChannel;
            var prefix = guildChannel?.Guild == null
                             ? ServerConfig.DefaultPrefix : Globals.ServerConfigs[guildChannel.Guild.Id].CommandPrefix;

            if (!(message.HasMentionPrefix(Client.CurrentUser, ref argPos) ||
                  message.HasStringPrefix(prefix, ref argPos)))
                return;

            var ctx = new SocketCommandContext(Client, message);
            var result = await CommandService.ExecuteAsync(ctx, argPos, Services, MultiMatchHandling.Best).ConfigureAwait(false);
            if (!result.IsSuccess) {
                if (result.Error.GetValueOrDefault() == CommandError.UnknownCommand) {
#if DEBUG
                    await ctx.Channel.SendMessageAsync($"{result.ErrorReason}").ConfigureAwait(false);
#endif
                    return;
                }
                var loggingChannel = Client.GetChannel(Globals.BotConfig.LogChannel) as SocketTextChannel;
                await ReportCommandErrorAsync(loggingChannel, ctx, result).ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}").ConfigureAwait(false);
            }
        }

        public async virtual Task InstallAsync() {
            await CommandService.AddModulesAsync(typeof(BotBase).GetTypeInfo().Assembly).ConfigureAwait(false);
            await CommandService.AddModulesAsync(Assembly.GetEntryAssembly()).ConfigureAwait(false);
            Client.MessageReceived += HandleCommandAsync;
        }

        public async Task ReportCommandErrorAsync(IMessageChannel logChannel, ICommandContext ctx, IResult result) {
            var eb = new EmbedBuilder()
                .WithAuthor(ctx.User)
                .WithColor(new Color(255, 0, 0))
                .WithTitle(result.ErrorReason)
                .WithDescription(ctx.Message.Content)
                .WithCurrentTimestamp()
                .AddField("Caller:", $"{ctx.User} | {ctx.User.Id}")
                .AddField($"Guild:", $"{ctx.Guild?.Name ?? "DM"} | {ctx.Guild?.Id.ToString() ?? ""}")
                .AddField($"Channel:", $"{ctx.Channel.Name} | {ctx.Channel.Id}");
            await logChannel.SendMessageAsync("", embed: eb.Build());
        }

    }
}