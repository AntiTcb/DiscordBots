#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/30/2016 2:25 PM
// Last Revised: 10/30/2016 2:27 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Modules.Utility {
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;

    #endregion

    public enum DeleteStrategy {
        BulkDelete = 0,
        Manual = 1
    }

    public enum DeleteType {
        Self = 0,
        Bot = 1,
        All = 2
    }

    public class UtilityModule : ModuleBase {
        #region Public Methods

        [Command("purge"), Alias("clean", "cleanup", "prune"), Summary("Cleans the bot's messages"),
         RequirePermission(ChannelPermission.ManageMessages)]
        public async Task CleanAsync
        ([Summary("The optional number of messages to delete; defaults to 10")] int count = 10,
         [Summary("The type of messages to delete - Self, Bot, or All")] DeleteType deleteType = DeleteType.Self,
         [Summary("The strategy to delete messages - BulkDelete or Manual")] DeleteStrategy deleteStrategy =
             DeleteStrategy.BulkDelete) {
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
                             await EndCleanAsync(deleteMessages, deleteStrategy).ConfigureAwait(false);
                             return;
                         }
                         deleteMessages.Add(msg);
                         index++;
                     }
                 }).ConfigureAwait(false);
        }

        #endregion Public Methods

        #region Internal Methods

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

        #endregion Internal Methods
    }
}