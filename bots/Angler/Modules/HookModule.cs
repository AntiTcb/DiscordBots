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

        [Command("hook")]
        [RequireOwner(Group = "A"), RequireContext(ContextType.DM, Group = "A")]
        [Summary("Add a webhook for the desired website, so you start receiving messages for new posts.")]
        public async Task AddWebhookAsync(Website site, Uri webhookUrl)
        {
            //await Context.Message.DeleteAsync();
            if (HookService.AddWebhook(site, webhookUrl.ToString()))
            {
                await ReplyAsync($"Added a webhook for {site}!");
                await HookService.GetWebhook(webhookUrl.ToString()).GetClient().SendMessageAsync($"This is a test message. If you can see it, you're all set up to receive messages from me for new posts on {site}. 😃");
            }
            else
                await ReplyAsync($"Couldn't add a webhook for {site}.");
        }

        [Command("updateHook"), RequireOwner]
        public async Task UpdateWebhookAsync(Uri webhookUrl, Website site)
        {
            if (HookService.UpdateWebhook(Webhook.ParseUrl(webhookUrl.ToString()).Id, site))
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
            var (webhooks, dbHooks) = HookService.ListWebhooks();
            string output = string.Join("\n", webhooks.Select(x => $"{x.Id} : {x.Site}"));
            await ReplyAsync(output);
        }

        [Command("testallwhs"), RequireOwner]
        public async Task TestWebhooksAsync(string msg)
        {
            await HookService.FireWebhooksAsync(Website.YGOrg, Context.Message);
            await HookService.ListWebhooks().webhooks.First().GetClient().SendMessageAsync(msg);
        }

        [Command("unhook")]
        [RequireOwner(Group = "A"), RequireContext(ContextType.DM, Group = "A")]
        public async Task RemoveWebhookAsync(Uri webhookUrl)
        {
            //await Context.Message.DeleteAsync();
            if (HookService.RemoveWebhook(Webhook.ParseUrl(webhookUrl.ToString()).Id))
                await ReplyAsync("Removed the webhook!");
            else
                await ReplyAsync("Couldn't remove the webhook.");
        }
    }
}
