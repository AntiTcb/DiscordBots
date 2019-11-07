using System.Threading.Tasks;
using Discord.Commands;

namespace OrgBot
{
    public class YugipediaModule : ModuleBase<SocketCommandContext>
    {
        public YugipediaService Yugipedia { get; set; }

        [Command("card"), Alias("c"), Summary("Searches Yugipedia and returns card information."), Remarks("card sangan")]
        public async Task GetCardAsync([Summary("Card name, case insensitive"), Remainder] string cardName)
        {

        }
    }
}
