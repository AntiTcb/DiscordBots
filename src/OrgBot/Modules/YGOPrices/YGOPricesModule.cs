// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 10/30/2016 2:08 PM
// Last Revised: 10/30/2016 2:08 PM
// Last Revised by: Alex Gravely

namespace OrgBot.Modules.YGOPrices
{
    using Discord.Commands;
    using Entities;
    using System;
    using System.Threading.Tasks;

    [Name("Yu-Gi-Oh! Prices"), Group("ygoprice")]
    public class YGOPricesModule : ModuleBase
    {
        [Command("card"), Alias("c"), Summary("Looks up the top 5 prices of a Yu-Gi-Oh! card. Names must be exact."), Remarks("price c sangan")]
        public async Task GetCardPriceAsync([Summary("Card name. Must be exact."), Remainder] string cardName)
        {
            using (Context.Channel.EnterTypingState())
            {
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
        }

        //[Command("falling")]
        //public async Task GetFallingPricesAsync() {
        //    throw new NotImplementedException();
        //}

        //[Command("rising")]
        //public async Task GetRisingPricesAsync() {
        //    throw new NotImplementedException();
        //}

        //[Command("topX"), Alias("top")]
        //public async Task GetTopPricesAsync(int topCount) {
        //    throw new NotImplementedException();
        //}
    }
}