#region Header
// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/30/2016 2:08 PM
// Last Revised: 10/30/2016 2:08 PM
// Last Revised by: Alex Gravely
#endregion
namespace OrgBot.Modules.YGOPrices {
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Entities;

    [Name("Yu-Gi-Oh Prices"), Group("price")]
    public class YGOPricesModule : ModuleBase {
        [Command("card"), Alias("c")]
        public async Task GetCardPriceAsync([Remainder] string cardName) {             
            var card = await YGOPricesClient.GetCardAndPriceResponseAsync(cardName);

            if (card == null)
            {
                await ReplyAsync("Card was null. Data could not be found.");
                return;
            }
            var em = card?.ToDiscordEmbed();
            em.WithUrl(Uri.EscapeUriString("http://yugiohprices.com/card_price?name={cardName}"));
            await ReplyAsync("", embed: em);
        }

        [Command("topX"), Alias("top")]
        public async Task GetTopPricesAsync(int topCount) {
            throw new NotImplementedException();
        }

        [Command("rising")]
        public async Task GetRisingPricesAsync() {
            throw new NotImplementedException();
        }

        [Command("falling")]
        public async Task GetFallingPricesAsync() {
            throw new NotImplementedException();
        }
    }
}