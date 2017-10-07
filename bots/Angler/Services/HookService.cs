using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Angler.Extensions;
using Discord;
using DiscordBCL.Services;

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

        public async Task FireWebhooksAsync(Website site, IMessage message)
        {
            var whs = _webhooks.Where(x => x.Value.Site == site).Select(x => x.Value);

            foreach (var wh in whs)
                await wh.GetClient().SendMessageAsync(message);
        }

        public (IEnumerable<Webhook> webhooks, IEnumerable<Webhook> dbHooks) ListWebhooks() 
            => (_webhooks.Values, _dbService.GetAll<Webhook>());

        public bool RemoveWebhook(ulong webhookId) => 
            _webhooks.TryRemove(webhookId, out var _) && _dbService.Remove<Webhook>(x => x.Id == webhookId);
    }
}
