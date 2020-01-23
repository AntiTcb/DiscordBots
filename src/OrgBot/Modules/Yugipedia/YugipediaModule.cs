using System;
using System.Threading.Tasks;
using Discord;
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
                try
                {
                    var card = await Yugipedia.GetCardAsync(cardName); 
                    if (card is null)
                        await ReplyAsync("Couldn't find a matching card in the TCG/OCG.");
                    else
                        await ReplyAsync(embed: card.ToEmbed());
                }
                catch (TimeoutException e) when (e.Message == "The Yugipedia API timed out; please try again.")
                {
                    await ReplyAsync(e.Message);

                    var eb = new EmbedBuilder
                    {
                        Title = "Yugipedia timeout",
                        Description = e.Data["requestProcess"].ToString()
                    }
                    .AddField("Caller", Context.User.ToString(), true)
                    .AddField("Guild", Context.IsPrivate ? Context.Guild.ToString() : "DM")
                    .AddField("Channel", Context.Channel.Id);

                    await (Context.Client.GetChannel(229841705128427532) as IMessageChannel).SendMessageAsync(embed: eb.Build());
                }
            }
        }
    }
}
