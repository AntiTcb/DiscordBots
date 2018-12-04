namespace OrgBot.Modules.YGOCard
{
    using System.Threading.Tasks;
    using Discord.Commands;

    [Name("Yugipedia")]
    public partial class YGOCardModule : ModuleBase<SocketCommandContext>
    {
        [Command("card"), Alias("c"), Summary("Gets the page url from Yugipedia."), Remarks("card sangan")]
        public async Task GetCardAsync([Summary("Card name, case insensitive"), Remainder] string cardName)
        {
            if (string.IsNullOrWhiteSpace(cardName))
                await ReplyAsync("https://yugipedia.com");
            else
                await ReplyAsync($"https://yugipedia.com/wiki/{cardName.Replace(' ', '_')}");
        }
    }
}