using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Angler.Extensions;
using Discord;
using DiscordBCL.Services;
using Serilog;

namespace Angler.Services
{
    public class HookService
    {
        private ConcurrentDictionary<ulong, Webhook> _webhooks;
        private readonly LiteDbService _dbService;

        public HookService(LiteDbService liteDb)
        {
            _dbService = liteDb;
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

            foreach (var wh in whs)
            {
                try
                {
                    await wh.GetClient().SendMessageAsync(message);
                }
                catch (Exception e)
                {
                    Log.Debug(e.ToString());
                }
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

            foreach (var wh in whs)
                await wh.GetClient().SendMessageAsync($"{Format.Bold("New Post!")}\n{url}", username: "OrgBot Jr.", avatarUrl: avatar);
        }

        public (IEnumerable<Webhook> webhooks, IEnumerable<Webhook> dbHooks) ListWebhooks() 
            => (_webhooks.Values, _dbService.GetAll<Webhook>());

        public bool RemoveWebhook(ulong webhookId) => 
            _webhooks.TryRemove(webhookId, out var _) && _dbService.Remove<Webhook>(x => x.Id == webhookId);
    }
}
