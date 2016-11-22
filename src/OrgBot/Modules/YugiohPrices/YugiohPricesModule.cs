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
namespace OrgBot.Modules.YugiohPrices {
    using System;
    using System.Threading.Tasks;
    using Discord.Commands;

    [Name("YGO Prices"), Group("price"), DontAutoLoad]
    public class YugiohPricesModule : ModuleBase {
        [Command("card"), Alias("c")]
        public async Task GetCardPriceAsync([Remainder] string cardName ) {
            throw new NotImplementedException();
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