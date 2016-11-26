#region Header

// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 10/29/2016 6:36 PM
// Last Revised: 11/04/2016 12:49 AM
// Last Revised by: Alex Gravely

#endregion Header

namespace OrgBot.Modules.YGOWikia {

    #region Using

    using Discord.Commands;
    using System.Threading.Tasks;
    using Entities;

    #endregion Using

    [Name("Yu-Gi-Oh! Wikia"), Group("wikia")]
    public class YGOWikiaModule : ModuleBase {

        #region Public Methods

        [Command("card"), Alias("c"), Summary("Gets card information from the Yu-Gi-Oh! wikia"), Remarks("wikia card sangan")]
        public async Task GetCardAsync([Remainder] string cardName) {
            using (Context.Channel.EnterTypingState()) {
                var card = await YGOWikiaClient.GetCardAsync(cardName);
                if (card == null) {
                    await ReplyAsync("Couldn't find card information to parse.");
                    return;
                }
                if (card.Name == "Parse failed") {
                    await ReplyAsync($"Could not parse card information. Card name needs more precision, or page HTML is invalid. Attempted to parse information from: <{card.Url}>");
                    return;
                }
                await ReplyAsync("", embed: card?.ToDiscordEmbed());
            }
        }

        #endregion Public Methods
    }
}