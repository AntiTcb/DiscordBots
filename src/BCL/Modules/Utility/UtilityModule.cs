// Description:
//
// Solution: DiscordBots
// Project: BCL
//
// Created: 10/30/2016 2:25 PM
// Last Revised: 11/04/2016 1:12 AM
// Last Revised by: Alex Gravely

namespace BCL.Modules.Utility {

    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public enum DeleteStrategy {
        BulkDelete = 0,
        Manual = 1
    }

    public enum DeleteType {
        Self = 0,
        Bot = 1,
        All = 2
    }

    [Name("Utility")]
    public class UtilityModule : ModuleBase {

        [Command("purge"), Alias("clean", "cleanup", "prune"), Summary("Cleans the bot's messages"),
            RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task CleanAsync
        ([Summary("The optional number of messages to delete; defaults to 10")] int count = 10,
         [Summary("The type of messages to delete - Self, Bot, or All")] DeleteType deleteType = DeleteType.Self,
         [Summary("The strategy to delete messages - BulkDelete or Manual")] DeleteStrategy deleteStrategy =
             DeleteStrategy.BulkDelete) {
            if (Context.User.Id != Globals.OWNER_ID || !(Context.User as SocketGuildUser).GetPermissions((Context.Channel as SocketGuildChannel)).ManageMessages) { return; }
            var index = 0;
            var deleteMessages = new List<IMessage>(count);
            var messages = Context.Channel.GetMessagesAsync();
            await messages.ForEachAsync
                (async m => {
                    IEnumerable<IMessage> delete = null;
                    switch (deleteType) {
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

                    foreach (var msg in delete.OrderByDescending(msg => msg.Timestamp)) {
                        if (index >= count) {
                            try {
                                await EndCleanAsync(deleteMessages, deleteStrategy).ConfigureAwait(false);
                            }
                            catch (Exception e) {
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

        [Command("requesthelp"), Alias("summonOwner", "reportbug"), RequireContext(ContextType.Guild),
                    Summary("Gives Anti-Tcb an alert that something is wrong, and an invite to the guild to provide assitance."), Remarks("summonOwner")]
        public async Task RequestOwnerAsync() {
            var owner = await Context.Client.GetUserAsync(Globals.OWNER_ID);
            await ReplyAsync("Summoning Anti-Tcb...");
            var loggingChannel = await Context.Client.GetChannelAsync(Globals.BotConfig.LogChannel) as ITextChannel;
            var invite = (Context.Channel as IGuildChannel).CreateInviteAsync(maxUses: 1);
            var summonMsg = $"{owner.Mention}: You're being summoned by {Context.User} to {Context.Guild.Name}. {invite}";
            await loggingChannel.SendMessageAsync(summonMsg);
        }

        internal async Task EndCleanAsync(IEnumerable<IMessage> messages, DeleteStrategy strategy) {
            switch (strategy) {
                case DeleteStrategy.BulkDelete:
                    await Context.Channel.DeleteMessagesAsync(messages).ConfigureAwait(false);
                    break;

                case DeleteStrategy.Manual:
                    foreach (var msg in messages.Cast<IUserMessage>()) {
                        await msg.DeleteAsync().ConfigureAwait(false);
                    }
                    break;
            }
        }
    }
}