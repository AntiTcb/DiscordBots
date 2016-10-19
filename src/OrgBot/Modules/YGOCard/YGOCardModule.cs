﻿#region Header

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
    using System.Threading.Tasks;
    using Discord.Commands;
    using Entities;

    #endregion

    public class YGOCardModule : ModuleBase {
        #region Public Methods

        [Command("card"), Alias("c")]
        public async Task GetCardAsync(string cardName) {
            if (cardName == "") {
                await ReplyAsync("I need a card name!");
                return;
            }
            var card = YGOCardAPIClient.Cards.FindCards(cardName).FirstOrDefault();
            if (card == null) {
                await ReplyAsync("Card not found");
                return;
            }
            await ReplyAsync(card.ToDiscordMessage());
        }

        #endregion Public Methods
    }
}