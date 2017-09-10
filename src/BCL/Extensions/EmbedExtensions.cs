using Discord;

namespace BCL.Extensions
{
    public static class EmbedExtensions
    {
        public static EmbedAuthorBuilder AsUser(this EmbedAuthorBuilder builder, IUser user) {
            if (user is IGuildUser) { builder.WithName((user as IGuildUser).Nickname ?? user.Username); }
            else {
                builder.WithName(user.Username);
            }
            builder.WithIconUrl(user.GetAvatarUrl());
            return builder; 
        }

        //public static EmbedBuilder WithAuthor(this EmbedBuilder eb, IUser user) =>
        //    eb.WithAuthor((a) => a.AsUser(user));

        public static EmbedBuilder WithMessage(this EmbedBuilder eb, IMessage msg) =>
            eb.WithAuthor(a => a.AsUser(msg.Author))
              .WithDescription(msg.Content);
    }
}
