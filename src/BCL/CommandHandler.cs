#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/26/2016 11:17 PM
// Last Revised: 11/04/2016 1:35 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL {
    #region Using

    using System.Reflection;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Interfaces;
    using Extensions;

    #endregion

    public class CommandHandler : ICommandHandler {
        #region Implementation of ICommandHandler

        public DiscordSocketClient Client { get; set; }
        public IDependencyMap Map { get; set; }
        public CommandService Service { get; set; }

        public CommandHandler()
        {
            Service = new CommandService(new CommandServiceConfig { CaseSensitiveCommands = false });
        }

        public async virtual Task HandleCommandAsync(SocketMessage msg) {
            var message = msg as SocketUserMessage;
            if (message == null || msg.Author.Id == Client.CurrentUser.Id) {
                return;
            }

            var argPos = 0;
            var guildChannel = message.Channel as SocketGuildChannel;
            var prefix = guildChannel?.Guild == null
                             ? ServerConfig.DefaultPrefix : Globals.ServerConfigs[guildChannel.Guild.Id].CommandPrefix;

            if (
                !(message.HasMentionPrefix(Client.CurrentUser, ref argPos) ||
                  message.HasStringPrefix(prefix, ref argPos))) {
                return;
            }
            var ctx = new CommandContext(Client, message);
            var result = await Service.ExecuteAsync(ctx, argPos, Map, MultiMatchHandling.Best).ConfigureAwait(false);
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

        public async virtual Task InstallAsync(IDependencyMap map = null) {
            Map = map ?? new DependencyMap();
            Client = Map.Get<DiscordSocketClient>();
            Map.Add(Service);
            await Service.AddModulesAsync(typeof(BotBase).GetTypeInfo().Assembly).ConfigureAwait(false);
            await Service.AddModulesAsync(Assembly.GetEntryAssembly()).ConfigureAwait(false);
            Client.MessageReceived += HandleCommandAsync;
        }

        public async Task ReportCommandErrorAsync(IMessageChannel logChannel, CommandContext ctx, IResult result) {
            var eb = new EmbedBuilder()
                .WithAuthor(ctx.User)
                .WithColor(new Color(1, 0, 0))
                .WithTitle(ctx.Message.Content)
                .WithDescription(result.ErrorReason)
                .AddField((f) =>
                    f.WithName("Caller:")
                     .WithValue($"{ctx.User} | {ctx.User.Id}")
                )
                .AddField((f) =>
                    f.WithName($"Guild:")
                     .WithValue($"{ctx.Guild?.Name ?? "DM"} | {ctx.Guild?.Id.ToString() ?? ""}")
                )
                .AddField((f) =>
                    f.WithName($"Channel:")
                     .WithValue($"{ctx.Channel.Name} | {ctx.Channel.Id}")
                );
            await logChannel.SendMessageAsync("", embed: eb);
        }

        #endregion Implementation of ICommandHandler
    }
}