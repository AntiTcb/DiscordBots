#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/26/2016 11:17 PM
// Last Revised: 10/13/2016 7:15 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL {
    #region Using

    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Discord.WebSocket;
    using Interfaces;

    #endregion

    public class CommandHandler : ICommandHandler {
        #region Implementation of ICommandHandler

        public IBotConfig BotConfig { get; set; }
        public DiscordSocketClient Client { get; set; }
        public Dictionary<ulong, IServerConfig> ServerConfigs { get; set; }
        public CommandService Service { get; set; }

        public async virtual Task HandleCommandAsync(CommandContext ctx) {
            if (ctx.User.IsBot) {
                return;
            }

            var argPos = 0;
            var prefix = ctx.Guild == null ? ServerConfig.DefaultPrefix : ServerConfigs[ctx.Guild.Id].CommandPrefix;
            bool isCharPrefix = false, isMentionPrefix = false;

            if (!string.IsNullOrEmpty(prefix.ToString())) {
                isCharPrefix = ctx.Message.HasCharPrefix(prefix, ref argPos);
            }
            else {
                isMentionPrefix = ctx.Message.HasMentionPrefix(Client.CurrentUser, ref argPos);
            }
            if (isCharPrefix || isMentionPrefix) {
                var result = await Service.Execute(ctx, argPos).ConfigureAwait(false);
                if (!result.IsSuccess) {
                    await ctx.Channel.SendMessageAsync(result.ErrorReason).ConfigureAwait(false);
                }
            }
        }

        public async Task InstallAsync(DiscordSocketClient c,IBotConfig botConfig,
             Dictionary<ulong, IServerConfig> serverConfigs, DependencyMap map = null) {
            Client = c;
            BotConfig = botConfig;
            Service = new CommandService();
            if (map == null) {
                map = new DependencyMap();
            }

            map.Add(Client);
            map.Add(BotConfig);

            await Service.AddModules(Assembly.GetEntryAssembly(), map).ConfigureAwait(false);
        }

        #endregion Implementation of ICommandHandler
    }
}