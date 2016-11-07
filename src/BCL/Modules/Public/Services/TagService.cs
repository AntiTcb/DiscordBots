#region Header
// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 11/06/2016 2:16 PM
// Last Revised: 11/06/2016 2:16 PM
// Last Revised by: Alex Gravely
#endregion
namespace BCL.Modules.Public.Services {
    using System.Threading.Tasks;
    using Discord.Commands;

    public static class TagService {
        public static async Task ReadTagAsync(CommandContext ctx, string tagName) {
            using (ctx.Channel.EnterTypingState()) {
                string tagValue;
                var tagExists = Globals.ServerConfigs[ctx.Guild.Id].Tags.TryGetValue(tagName, out tagValue);

                if (tagExists) {
                    await ctx.Channel.SendMessageAsync(tagValue);
                }
                else {
                    await ctx.Channel.SendMessageAsync("Tag not found.");
                }
            }
        }

        public static async Task CreateTagAsync(CommandContext ctx, string tagName, string tagValue) {
            Globals.ServerConfigs[ctx.Guild.Id].Tags.Add(tagName, tagValue);
        }
    }
}