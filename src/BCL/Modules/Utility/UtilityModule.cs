namespace BCL.Modules.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;

    public enum DeleteStrategy
    {
        BulkDelete = 0,
        Manual = 1
    }

    public enum DeleteType
    {
        Self = 0,
        Bot = 1,
        All = 2
    }

    [Name("Utility")]
    public class UtilityModule : ModuleBase<SocketCommandContext>
    {
        [Command("purge"), Alias("clean", "cleanup", "prune"), Summary("Cleans the bot's messages")]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [RequireUserPermission(ChannelPermission.ManageMessages, Group = "A"), RequireOwner(Group = "A")]
        public async Task CleanAsync
        ([Summary("The optional number of messages to delete; defaults to 10")] int count = 10,
         [Summary("The type of messages to delete - Self, Bot, or All")] DeleteType deleteType = DeleteType.Self,
         [Summary("The strategy to delete messages - BulkDelete or Manual")] DeleteStrategy deleteStrategy =
             DeleteStrategy.BulkDelete)
        {
            var index = 0;
            var deleteMessages = new List<IMessage>(count);
            var messages = Context.Channel.GetMessagesAsync();
            await messages.ForEachAsync
                (async m =>
                {
                    IEnumerable<IMessage> delete = null;
                    switch (deleteType)
                    {
                        case DeleteType.Self:
                            delete = m.Where(msg => msg.Author.Id == Context.Client.CurrentUser.Id);
                            break;

                        case DeleteType.Bot:
                            delete = m.Where(msg => msg.Author.IsBot);
                            break;

                        case DeleteType.All:
                            delete = m;
                            break;
                    }

                    foreach (var msg in delete.OrderByDescending(msg => msg.Timestamp))
                    {
                        if (index >= count)
                        {
                            try
                            {
                                await EndCleanAsync(deleteMessages, deleteStrategy).ConfigureAwait(false);
                            }
                            catch (Exception e)
                            {
                                await ReplyAsync($"Error! {e.Message}").ConfigureAwait(false);
                                return;
                            }
                            return;
                        }
                        deleteMessages.Add(msg);
                        index++;
                    }
                }).ConfigureAwait(false);

            await Context.Message.DeleteAsync();
        }

        [Command("requesthelp"), Alias("summonOwner", "reportbug"), RequireContext(ContextType.Guild)]
        [RequireBotPermission(ChannelPermission.CreateInstantInvite)]
        [Summary("Gives AntiTcb an alert that something is wrong, and an invite to the guild to provide assistance."), Remarks("summonOwner")]
        public async Task RequestOwnerAsync()
        {
            var owner = Context.Client.GetUser(Globals.OWNER_ID);
            await ReplyAsync("Summoning AntiTcb...");
            var loggingChannel = Context.Client.GetChannel(Globals.BotConfig.LogChannel) as ITextChannel;
            var invite = await (Context.Channel as INestedChannel).CreateInviteAsync(maxUses: 1);
            var summonMsg = $"{owner.Mention}: You're being summoned by {Context.User} to {Context.Guild.Name}. {invite.Url}";
            await loggingChannel.SendMessageAsync(summonMsg);
        }

        internal async Task EndCleanAsync(IEnumerable<IMessage> messages, DeleteStrategy strategy)
        {
            switch (strategy)
            {
                case DeleteStrategy.BulkDelete:
                    await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages).ConfigureAwait(false);
                    break;

                case DeleteStrategy.Manual:
                    foreach (var msg in messages.Cast<IUserMessage>())
                    {
                        await msg.DeleteAsync().ConfigureAwait(false);
                    }
                    break;
            }
        }
    }
}