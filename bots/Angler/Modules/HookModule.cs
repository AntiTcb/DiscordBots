using System;
using System.Linq;
using System.Threading.Tasks;
using Angler.Services;
using Discord;
using Discord.Commands;
using DiscordBCL;

namespace Angler.Modules
{
    public class HookModule : ModuleBase<ShardedCommandContext>
    {
        public HookService HookService { get; set; }

        [Command("new"), RequireRole(92859566114504704)]
        public async Task PushManualWebhookAsync(Uri url)
        {
            if (url.Host.Contains("ygorganization"))
            {
                await HookService.FireWebhooksAsync(Website.YGOrg, url.ToString());
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
            else if (url.Host.Contains("cardfightcoalition"))
            {
                await HookService.FireWebhooksAsync(Website.CardCoal, url.ToString());
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
            else
            {
                await ReplyAsync("That URL was not for YGOrganization or Cardfight Coalition.");
            }
        }

        [Command("hook"), RequireOwner]
        public async Task AddWebhookAsync(Website site, string webhookUrl)
        {
            //await Context.Message.DeleteAsync();
            if (HookService.AddWebhook(site, webhookUrl))
                await ReplyAsync($"Added a webhook for {site}!");
            else
                await ReplyAsync($"Couldn't add a webhook for {site}.");
        }

        [Command("updateHook"), RequireOwner]
        public async Task UpdateWebhookAsync(string webhookUrl, Website site)
        {
            if (HookService.UpdateWebhook(Webhook.ParseUrl(webhookUrl).Id, site))
                await ReplyAsync("Updated!");
            else
                await ReplyAsync("Couldn't update.");
        }

        [Command("purgeCache"), RequireOwner]
        public async Task PurgeCacheAsync()
        {
            HookService.PurgeCache();
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }

        [Command("listWhs"), RequireOwner]
        public async Task ListWebhooks()
        {
            var whs = HookService.ListWebhooks();
            string output = string.Join("\n", whs.webhooks.Select(x => $"{x.Id} : {x.Site}"));
            await ReplyAsync(output);
        }

        [Command("testwh"), RequireOwner]
        public async Task TestWebhooksAsync(string msg)
        {
            await HookService.FireWebhooksAsync(Website.YGOrg, Context.Message);
            await HookService.ListWebhooks().webhooks.First().GetClient().SendMessageAsync(msg);
        }

        [Command("unhook"), RequireOwner]
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
