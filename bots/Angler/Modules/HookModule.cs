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
            if (HookService.TryAddWebhook(site, webhookUrl.ToString(), out var webhook))
            {
                try
                {
                    await webhook.GetClient().SendMessageAsync($"This is a test message. If you can see it, you're all set up to receive messages from me for new posts on {site}. 😃");
                    await ReplyAsync($"Added a webhook for {site}!");
                }
                catch (InvalidOperationException)
                {
                    HookService.RemoveWebhook(webhook.Id);
                    await ReplyAsync($"Couldn't add a webhook for {site}. Webhook URL was invalid.");
                }
            }
            else
                await ReplyAsync($"Couldn't add a webhook for {site}.");
        }

        [Command("get"), RequireOwner]
        public async Task GetWebhookAsync(ulong id)
        {
            var webhook = HookService.GetWebhook(id);

            if (webhook is null) return;

            await ReplyAsync($"https://discordapp.com/api/webhooks/{webhook.Id}/{webhook.Token}");
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

        [Command("testwh"), RequireOwner]
        public async Task TestWebhookAsync(ulong webhookId, [Remainder] string msg)
        {
            var wh = HookService.GetWebhook(webhookId).GetClient();

            await wh.SendMessageAsync(msg);

            await ReplyAsync("Tested webhook!");
        }

        [Command("testwh"), RequireOwner]
        public Task TestWebhookAsync(Uri webhookUrl, [Remainder] string msg)
            => TestWebhookAsync(Webhook.ParseUrl(webhookUrl.ToString()).Id, msg);

        [Command("testallwhs"), RequireOwner]
        public async Task TestWebhooksAsync([Remainder] string msg)
        {
            await HookService.FireWebhooksAsync(Website.YGOrg, Context.Message);
            await HookService.ListWebhooks().webhooks.First().GetClient().SendMessageAsync(msg);
        }

        [Command("unhook"), Priority(1)]
        [RequireOwner(Group = "A"), RequireContext(ContextType.DM, Group = "A")]
        public Task RemoveWebhookAsync(Uri webhookUrl)
            => RemoveWebhookAsync(Webhook.ParseUrl(webhookUrl.ToString()).Id);

        [Command("unhook"), Priority(2)]
        [RequireOwner(Group = "A"), RequireContext(ContextType.DM, Group = "A")]
        public async Task RemoveWebhookAsync(ulong webhookId)
        {
            if (HookService.RemoveWebhook(webhookId))
                await ReplyAsync("Removed the webhook!");
            else
                await ReplyAsync("Couldn't remove the webhook.");
        }
    }
}
