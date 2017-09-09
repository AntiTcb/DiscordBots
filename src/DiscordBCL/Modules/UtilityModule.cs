using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBCL.Modules
{
    [Name("Utility")]
    public class UtilityModule : ModuleBase<ShardedCommandContext>
    {
        [Command("clean", RunMode = RunMode.Async), Alias("purge")]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [RequireUserPermission(ChannelPermission.ManageMessages, Group = "A"), RequireOwner(Group = "A")]
        [Summary("Cleans up messages from the channel.")]
        [Remarks("clean 10")]
        public async Task CleanAsync(
            [Summary("The amount of messages to delete.")] int amount = 10,
            [Summary("Whose messages should be deleted.")] DeleteTarget target = DeleteTarget.Bot,
            [Summary("How the messages should be deleted.")] DeleteStrategy strategy = DeleteStrategy.Bulk)
        {
            int index = 0;
            var messagesToDelete = new List<IMessage>(amount);
            var messages = Context.Channel.GetMessagesAsync();

            await messages.ForEachAsync(
                async m => 
                {
                    IEnumerable<IMessage> delete = null;
                    switch (target)
                    {
                        case DeleteTarget.Self:
                            delete = m.Where(msg => msg.Author.Id == Context.User.Id);
                            break;
                        case DeleteTarget.Bot:
                            delete = m.Where(msg => msg.Author.Id == Context.Client.CurrentUser.Id);
                            break;
                        case DeleteTarget.Bots:
                            delete = m.Where(msg => msg.Author.IsBot);
                            break;
                        case DeleteTarget.All:
                            delete = m;
                            break;
                    }

                    foreach (var msg in delete.OrderByDescending(x => x.Timestamp))
                    {
                        if (index >= amount)
                        {
                            try
                            {
                                await CleanAsync(messagesToDelete, strategy).ConfigureAwait(false);
                            }
                            catch (Exception e)
                            {
                                await ReplyAsync($"Error! {e.Message}").ConfigureAwait(false);
                                return;
                            }
                            return;
                        }
                        messagesToDelete.Add(msg);
                        index++;
                    }
                }).ConfigureAwait(false);
            await Context.Message.DeleteAsync().ConfigureAwait(false);
        }

        [Command("requesthelp")]
        [Alias("summonowner", "assistance", "pinganti")]
        [RequireContext(ContextType.Guild)]
        [RequireBotPermission(ChannelPermission.CreateInstantInvite)]
        [Summary("Provides AntiTcb an invite to the guild to answer your question about the bot!")]
        [Remarks("requesthelp")]
        public async Task RequestHelpAsync()
        {
            var antiGuildChannel = Context.Client.GetChannel(226558554662895618) as SocketTextChannel;
            var anti = antiGuildChannel.GetUser(89613772372574208);
            var invite = await (Context.Channel as ITextChannel).CreateInviteAsync(maxUses: 1, isTemporary: true).ConfigureAwait(false);

            await ReplyAsync("AntiTcb has been notified and should be in shortly.").ConfigureAwait(false);
            await antiGuildChannel.SendMessageAsync(
                $"{anti.Mention}: You're being summoned by {Context.User.Mention}" +
                $" to {Context.Guild.Name} / {Context.Channel.Name}. {invite.Url}").ConfigureAwait(false);
        }

        internal async Task CleanAsync(IEnumerable<IMessage> messages, DeleteStrategy strategy)
        {
            switch (strategy)
            {
                case DeleteStrategy.Bulk:
                    await Context.Channel.DeleteMessagesAsync(messages).ConfigureAwait(false);
                    break;

                case DeleteStrategy.Single:
                    foreach (var msg in messages)
                        await msg.DeleteAsync().ConfigureAwait(false);
                    break;
            }
        }
    }

    public enum DeleteTarget { Self, Bot, Bots, All }
    public enum DeleteStrategy { Bulk, Single }
}
