using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Webhook;

namespace Angler.Extensions
{
    public static class DiscordWebhookClientExtensions
    {
        public static Task SendMessageAsync(this DiscordWebhookClient client, IMessage msg)
            => client.SendMessageAsync(msg.Content, embeds: msg.Embeds.OfType<Embed>().ToArray(), username: msg.Author.Username, avatarUrl: msg.Author.GetAvatarUrl());
    }
}
