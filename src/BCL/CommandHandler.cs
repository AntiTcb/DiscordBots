#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 09/14/2016 4:15 PM
// Last Revised: 09/14/2016 4:15 PM
// Last Revised by: Alex Gravely
#endregion
namespace BCL {
    using System.Reflection;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Interfaces;

    public class CommandHandler : ICommandHandler {
        #region Implementation of ICommandHandler

        public CommandService Service { get; set; }
        public DiscordSocketClient Client { get; set; }
        public ISelfUser Self { get; set; }
        public IConfig Config { get; set; }

        public async Task Install(DiscordSocketClient c, IConfig config, DependencyMap map = null) {
            Client = c;
            Config = config;
            Service = new CommandService();
            Self = await Client.GetCurrentUserAsync();
            if (map == null) { map = new DependencyMap();}
            map.Add(Client);
            map.Add(Self);
            map.Add(Config);

            await Service.LoadAssembly(Assembly.GetEntryAssembly(), map);
        }

        public async virtual Task HandleCommand(IMessage paramMessage) {
            var msg = paramMessage as IUserMessage;
            if (msg == null) { return;}
            var argPos = 0;
            if (msg.HasMentionPrefix(Self, ref argPos) ||
                msg.HasCharPrefix(Config.CommandPrefix, ref argPos)) {
                var result = await Service.Execute(msg, argPos);
                if (!result.IsSuccess) {
                    await msg.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }

        #endregion
    }
}