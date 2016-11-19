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
    using Discord.Commands;
    using Discord.WebSocket;
    using Interfaces;

    #endregion

    public class CommandHandler : ICommandHandler {
        #region Implementation of ICommandHandler

        public DiscordSocketClient Client { get; set; }
        public IDependencyMap Map { get; set; }
        public CommandService Service { get; set; } = new CommandService();

        public async virtual Task HandleCommandAsync(SocketMessage msg) {
            var message = msg as SocketUserMessage;
            if (message == null) {
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
                    return;
                }
                var loggingChannel = Client.GetChannel(Globals.BotConfig.LogChannel) as SocketTextChannel;
                await
                    loggingChannel.SendMessageAsync
                        ($"**Command:** {ctx.Message}\n" + $"**Error:** {result.ErrorReason}\n" +
                         $"**Caller:** {ctx.User} ({ctx.User.Id}) / {ctx.Guild.Name} | {ctx.Channel.Name}").
                        ConfigureAwait(false);
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

        #endregion Implementation of ICommandHandler
    }
}