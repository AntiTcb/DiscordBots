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
    using System.Threading.Tasks;
    using BCL;
    using Discord;
    using Discord.Commands;

    public class SelfCommandHandler : CommandHandler {

        public async override Task HandleCommand(IMessage paramMessage) {
            var msg = paramMessage as IUserMessage;
            if (msg == null) {
                return;
            }
            var argPos = 0;
            if (msg.HasMentionPrefix(Self, ref argPos) || 
                msg.HasCharPrefix(Config.CommandPrefix, ref argPos)) {
                var result = await Service.Execute(msg, argPos);
                if (!result.IsSuccess) {
                    await msg.ModifyAsync(m => m.Content = result.ErrorReason);
                }
            }
        }
    }
}