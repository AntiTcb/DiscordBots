#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/29/2016 6:36 PM
// Last Revised: 11/04/2016 12:49 AM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot.Modules.YGOWikia {
    #region Using

    using System.Threading.Tasks;
    using Discord.Commands;
    using YGOCard.Entities;

    #endregion

    [Group("wikia")]
    public class YGOWikiaModule : ModuleBase {
        #region Public Methods

        [Command("card"), Alias("c")]
        public async Task GetCardAsync([Remainder] string cardName) {
            using (Context.Channel.EnterTypingState()) {
                var card = await YGOWikiaClient.GetCardAsync(cardName);
                if (card?.Name == "Parse failed.") {
                    await ReplyAsync($"Parsing of card failed. Card name needs more precision, or page HTML is invalid. <{card.Url}>");
                    return;
                }
                await ReplyAsync(card?.ToDiscordMessage() ?? "Invalid card name/Card not found.");
            }
        }

        #endregion Public Methods
    }
}