using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace BCL.Extensions
{
    public static class EmbedExtensions
    {
        public static EmbedAuthorBuilder AsUser(this EmbedAuthorBuilder builder, IUser user) {
            if (user is IGuildUser) { builder.WithName((user as IGuildUser).Nickname ?? user.Username); }
            else {
                builder.WithName(user.Username);
            }
            builder.WithIconUrl(user.AvatarUrl);
            return builder; 
        }

        public static EmbedBuilder WithAuthor(this EmbedBuilder eb, IUser user) =>
            eb.WithAuthor((a) => a.AsUser(user));

        public static EmbedBuilder WithMessage(this EmbedBuilder eb, IMessage msg) =>
            eb.WithAuthor(a => a.AsUser(msg.Author))
              .WithDescription(msg.Content);
    }
}
