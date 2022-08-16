using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Angler.Extensions;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBCL.Services;
using Humanizer;
using Serilog;

namespace Angler.Services
{
    public class HookService
    {
        private ConcurrentDictionary<ulong, Webhook> _webhooks;
        private readonly LiteDbService _dbService;
        private readonly DiscordShardedClient _client;

        public HookService(LiteDbService liteDb, DiscordShardedClient client)
        {
            _dbService = liteDb;
            _client = client;

            var whs = _dbService.GetAll<Webhook>();
            _webhooks = new ConcurrentDictionary<ulong, Webhook>(whs.Select(x => new KeyValuePair<ulong, Webhook>(x.Id, x)));

            foreach (var webhook in whs)
                webhook.GetClient();
        }

        public bool AddWebhook(Website site, string webhookUrl)
        {
            var wh = new Webhook(webhookUrl, site);
            _dbService.Add(wh);
            return _webhooks.TryAdd(wh.Id, wh);
        }

        public bool TryAddWebhook(Website site, string webhookUrl, out Webhook webhook)
        {
            var wh = new Webhook(webhookUrl, site);
            _dbService.Add(wh);

            if (_webhooks.TryAdd(wh.Id, wh))
            {
                webhook = wh;
                return true;
            }
            else
            {
                webhook = null;
                return false;
            }
        }

        public bool UpdateWebhook(ulong webhookId, Website site)
        {
            var wh = _dbService.Get<Webhook>(x => x.Id == webhookId);
            wh.Site = site;
            _webhooks[webhookId] = wh;
            return _dbService.Update(wh);
        }

        public void PurgeCache()
        {
            _webhooks = new ConcurrentDictionary<ulong, Webhook>(_dbService.GetAll<Webhook>().Select(x => new KeyValuePair<ulong, Webhook>(x.Id, x)));
        }

        public async Task FireWebhooksAsync(Website site, IMessage message)
        {
            var whs = _webhooks.Where(x => x.Value.Site == site || x.Value.Site == Website.All).Select(x => x.Value);

            var sb = new StringBuilder();

            foreach (var wh in whs)
            {
                try
                {
                    await wh.GetClient().SendMessageAsync(message);
                }
                catch (HttpException http) when (http.DiscordCode == DiscordErrorCode.UnknownWebhook)
                {
                    RemoveWebhook(wh.Id);
                    sb.AppendLine($"Webhook {wh.Id} has been deleted. Automatically removed it from the DB.");
                }
                catch (Exception e)
                {
                    Log.Debug(e.ToString());
                    sb.AppendLine($"Webhook: {wh.Id} || {e}");
                }
            }

            if (sb.Length > 0)
            {
                await (_client.GetChannel(363832894902501378) as SocketTextChannel).SendMessageAsync($"<@!89613772372574208> {sb.ToString().Truncate(2000)}");
            }
        }

        public async Task FireWebhooksAsync(Website site, string url)
        {
            var whs = _webhooks.Where(x => x.Value.Site == site || x.Value.Site == Website.All).Select(x => x.Value);

            string avatar = url.Contains("ygorganization")
                ? "https://ygorganization.com/wp-content/uploads/2014/09/TheOrgLogo.png"
                : url.Contains("cardfightcoalition")
                  ? "https://ygorganization.com/wp-content/uploads/2016/08/CCFBlogo-1.png"
                  : null;

            var msgs = whs.Select(wh => wh.GetClient().SendMessageAsync($"{Format.Bold("New Post!")}\n{url}", username: "OrgBot Jr.", avatarUrl: avatar));

            await Task.WhenAll(msgs);
        }

        public Webhook GetWebhook(ulong webhookId)
            => _webhooks.TryGetValue(webhookId, out var w) ? w : _dbService.Get<Webhook>(x => x.Id == webhookId);
        public Webhook GetWebhook(string url)
            => GetWebhook(Webhook.ParseUrl(url).Id);

        public (IEnumerable<Webhook> webhooks, IEnumerable<Webhook> dbHooks) ListWebhooks() 
            => (_webhooks.Values, _dbService.GetAll<Webhook>());

        public bool RemoveWebhook(ulong webhookId) => 
            _webhooks.TryRemove(webhookId, out var _) && _dbService.Remove<Webhook>(x => x.Id == webhookId);
    }
}
