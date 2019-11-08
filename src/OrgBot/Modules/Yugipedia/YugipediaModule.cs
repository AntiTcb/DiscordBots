using System.Threading.Tasks;
using Discord.Commands;

namespace OrgBot
{
    [Name("Yugipedia")]
    public class YugipediaModule : ModuleBase<SocketCommandContext>
    {
        public YugipediaService Yugipedia { get; set; }

        [Command("card"), Alias("c", "wikia card", "wikia c", "wiki card", "wiki c")]
        [Summary("Searches Yugipedia and returns card information."), Remarks("card sangan")]
        public async Task GetCardAsync([Summary("Card name, case insensitive"), Remainder] string cardName)
        {
            using (Context.Channel.EnterTypingState())
            {
                var card = await Yugipedia.GetCardAsync(cardName);
                if (card is null)
                    await ReplyAsync("Couldn't find a matching card in the TCG/OCG.");
                else
                    await ReplyAsync(embed: card.ToEmbed());
            }
        }
    }
}
