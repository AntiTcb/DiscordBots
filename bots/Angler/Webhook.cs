using System;
using System.Text.RegularExpressions;
using Discord;
using Discord.Rest;
using Discord.Webhook;
using LiteDB;

namespace Angler
{
    public class Webhook
    {
        static readonly Regex _webhookRegex = new Regex(@".*webhooks\/(\d*)\/(.*)");

        [BsonIgnore]
        private Lazy<DiscordWebhookClient> _client;
        
        public Website Site { get; set; }
        public ulong Id { get; set; }
        public string Token { get; set; }

        public Webhook() 
            => _client = new Lazy<DiscordWebhookClient>(() => new DiscordWebhookClient(Id, Token,
                    new DiscordRestConfig { LogLevel = LogSeverity.Debug }));

        public Webhook(ulong id, string token, Website site) : this()
        {
            Id = id;
            Token = token;
            Site = site;
        }

        public Webhook(string webhookUrl, Website site) : this()
        {
            var whTuple = ParseUrl(webhookUrl);
            Id = whTuple.Id;
            Token = whTuple.Token;
            Site = site;
        }

        public DiscordWebhookClient GetClient()
            => _client.Value;

        public static (ulong Id, string Token) ParseUrl(string url)
        {
            var groups = _webhookRegex.Match(url).Groups;
            ulong id = ulong.Parse(groups[1].Value);
            string token = groups[2].Value;
            return (id, token);
        }
    }
}
