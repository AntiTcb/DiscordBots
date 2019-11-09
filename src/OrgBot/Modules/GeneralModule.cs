using Discord.Commands;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrgBot.Modules
{
    internal class PostsModel
    {
        public string Link { get; set; }
    }
    [Name("General")]
    public class GeneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("site"), Summary("Returns the YGOrganization site url.")]
        [Remarks("site")]
        public async Task SiteAsync() 
            => await ReplyAsync("https://ygorganization.com");

        [Command("new", RunMode = RunMode.Async), Alias("latest", "article", "news")]
        [Summary("Returns the URL of the latest article on YGOrganization.com")]
        [Remarks("new")]
        public async Task GetNewestArticleAsync()
        {
            using (var c = new HttpClient())
            {
                string json = await c.GetStringAsync("https://ygorganization.com/wp-json/wp/v2/posts/?per_page=1");
                var dummyModel = JsonConvert.DeserializeObject<PostsModel[]>(json).FirstOrDefault();
                await ReplyAsync(dummyModel?.Link ?? "Latest article could not be found.");
            }
            
        }
    }
}
