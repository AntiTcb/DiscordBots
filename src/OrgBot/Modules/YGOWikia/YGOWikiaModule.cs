#region Header
// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/29/2016 6:36 PM
// Last Revised: 10/29/2016 6:36 PM
// Last Revised by: Alex Gravely
#endregion
namespace OrgBot.Modules.YGOWikia {
    using System.Threading.Tasks;
    using BCL.Preconditions;
    using Discord.Commands;
    using YGOCard.Entities;

    [Group("wikia")]
    public partial class YGOWikiaModule : ModuleBase{
        [Command("card"), Alias("c")]
        public async Task GetCardAsync([Remainder] string cardName) {
            await Context.Channel.TriggerTypingAsync();
            var card = await YGOWikiaClient.GetCardAsync(cardName);
            await ReplyAsync(card?.ToDiscordMessage() ?? "Invalid card name/Card not found");
        }
    }
}