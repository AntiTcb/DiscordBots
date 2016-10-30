#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/19/2016 12:23 AM
// Last Revised: 10/19/2016 12:56 AM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot.Modules.YGOCard {
    #region Using

    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Entities;

    #endregion

    public partial class YGOCardModule : ModuleBase {
        #region Public Methods

        [Command("card"), Alias("c")]
        public async Task GetCardAsync([Remainder] string cardName) {
            await Context.Channel.TriggerTypingAsync();
            if (cardName == "") {
                await ReplyAsync("I need a card name!");
                return;
            }
            var card = YGOCardAPIClient.Cards.FindCards(cardName.ToLower()).FirstOrDefault();
            if (card == null) {
                await ReplyAsync("Card not found.");
                return;
            }
            if (card.Name == "Spirit Elimination") {
                await ReplyAsync("NO! NO, NO, NO, NO, NO! NO! STOP ASKING QUESTIONS ABOUT THIS CARD!");
                await Task.Delay(4000);
            }
            if (card.Name.Contains("Bujin")) {
                await ReplyAsync("Bujin master race!");
            }
            await ReplyAsync(card.ToDiscordMessage());
        }

        [Command("cardupdate"), Alias("cu")]
        public async Task ForceCardUpdateAsync([Remainder] string cardName) {
            if (cardName == "") {
                await ReplyAsync("I need a card name!");
                return;
            }
            var card = YGOCardAPIClient.Cards.FindCards(cardName.ToLower()).FirstOrDefault();
            if (card == null)
            {
                await ReplyAsync("Card not found.");
                return;
            }
            await card.UpdateAsync();
            await ReplyAsync(":thumbsup:");
        }

        [Command("listcards")]
        public async Task GetCardsAsync([Remainder] string cardName) {
            await Context.Channel.TriggerTypingAsync();
            if (cardName == "") {
                await ReplyAsync("I need a card name!");
                return;
            }
            if (Context.Channel.Id == 87463676595949568) {
                await ReplyAsync("Command is disabled in this channel. Please use <#171483830845177857>");
                return;
            }
            var cards = YGOCardAPIClient.Cards.FindCards(cardName.ToLower());
            var listOfCards = string.Join(", ", cards.Select(x => x.Name));
            if (listOfCards.Length > 2000) {
                var splitList = Enumerable.Range(0, listOfCards.Length / 1900).
                                           Select(i => listOfCards.Substring(i * 1900, 1900));
                foreach (var list in splitList) {
                    await ReplyAsync(list);
                    await Task.Delay(1000);
                }
                return;
            }
            await ReplyAsync(listOfCards);
        }


        #endregion Public Methods
    }
}