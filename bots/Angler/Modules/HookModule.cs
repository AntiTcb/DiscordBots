using System.Linq;
using System.Threading.Tasks;
using Angler.Services;
using Discord.Commands;

namespace Angler.Modules
{
    public class HookModule : ModuleBase<ShardedCommandContext>
    {
        public HookService HookService { get; set; }
        
        [Command("hook")]
        public async Task AddWebhookAsync(Website site, string webhookUrl)
        {
            //await Context.Message.DeleteAsync();
            if (HookService.AddWebhook(site, webhookUrl))
                await ReplyAsync($"Added a webhook for {site}!");
            else
                await ReplyAsync($"Couldn't add a webhook for {site}.");
        }

        [Command("listWhs")]
        public async Task ListWebhooks()
        {
            var whs = HookService.ListWebhooks();
        }

        [Command("testwh")]
        public async Task TestWebhooksAsync(string msg)
        {
            await HookService.FireWebhooksAsync(Website.YGOrg, Context.Message);
            await HookService.ListWebhooks().webhooks.First().GetClient().SendMessageAsync(msg);
        }

        [Command("unhook")]
        public async Task RemoveWebhookAsync(string webhookUrl)
        {
            //await Context.Message.DeleteAsync();
            if (HookService.RemoveWebhook(Webhook.ParseUrl(webhookUrl).Id))
                await ReplyAsync("Removed the webhook!");
            else
                await ReplyAsync("Couldn't remove the webhook.");
        }
    }
}
