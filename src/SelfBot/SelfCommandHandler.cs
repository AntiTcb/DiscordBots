#region Header
// Description:
// 
// Solution: DiscordBots
// Project: SelfBot
// 
// Created: 10/04/2016 5:51 PM
// Last Revised: 10/04/2016 5:51 PM
// Last Revised by: Alex Gravely
#endregion

namespace SelfBot {
    using System.Reflection;
    using System.Threading.Tasks;
    using BCL;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;

    public class SelfCommandHandler : CommandHandler {
        #region Overrides of CommandHandler

        public async override Task HandleCommandAsync(SocketMessage msg) {
           var message = msg as SocketUserMessage;
            if (message == null || msg.Author.Id != 89613772372574208) {
                return;
            }

            var argPos = 0;
            var prefix = ">>>";
            if (!(message.HasMentionPrefix(Client.CurrentUser, ref argPos) ||
                  message.HasStringPrefix(prefix, ref argPos))) {
                return;
            }
            var ctx = new CommandContext(Client, message);
            var result = await Service.Execute(ctx, argPos, Map, MultiMatchHandling.Best);
            if (!result.IsSuccess) {
                return;
            }
        }

        public async override Task InstallAsync(IDependencyMap map = null) {
            Service = new CommandService();
            Map = map ?? new DependencyMap();
            Client = Map.Get<DiscordSocketClient>();
            Map.Add(Service);
            await Service.AddModules(typeof(BotBase).GetTypeInfo().Assembly).ConfigureAwait(false);
            await Service.AddModules(Assembly.GetEntryAssembly()).ConfigureAwait(false);
            Client.MessageReceived += HandleCommandAsync;
        }

        #endregion
    }
}