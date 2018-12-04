namespace OrgBot.Modules.YGOWikia {
    using BCL;
    using Discord;
    using Discord.Commands;
    using Entities;
    using System.Threading.Tasks;

    [Name("Yu-Gi-Oh! Wikia"), Group("wikia")]
    public class YGOWikiaModule : ModuleBase<SocketCommandContext> {

        [Command("card"), Alias("c"), Summary("Gets card information from the Yu-Gi-Oh! Wikia. Must be a real card. This command is being removed in a future update."), Remarks("wikia card sangan")]
        public async Task GetCardAsync([Remainder] string cardName)
        {
            using (Context.Channel.EnterTypingState())
            {
                var card = await YGOWikiaClient.GetCardAsync(cardName);
                if (card == null)
                {
                    await ReplyAsync("Couldn't find card information to parse.");
                    return;
                }
                if (card.Name == "Parse failed")
                {
                    await ReplyAsync($"Could not parse card information. Card name needs more precision, or page HTML is invalid. Attempted to parse information from: <{card.Url}>");
                    return;
                }
                var prefix = Context.IsPrivate ? ServerConfig.DefaultPrefix : Globals.ServerConfigs[Context.Guild.Id].CommandPrefix;
                await ReplyAsync(Format.Bold($"This command is being removed in a future update. Please begin using the {prefix}card command instead. The card embed for that command will be fully added in a future update."), embed: card?.ToDiscordEmbed().Build());
            }
        }
    }
}