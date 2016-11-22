#region Header

// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 10/19/2016 10:34 PM
// Last Revised: 10/19/2016 11:31 PM
// Last Revised by: Alex Gravely

#endregion Header

namespace OrgBot.Modules.YGOCard {

    #region Using

    using Discord.Commands;
    using Entities;
    using Preconditions;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    #endregion Using

    public partial class YGOCardModule {

        #region Public Structs + Classes

        [Name("Card-Editing"), Group("edit"), RequireContext(ContextType.Guild), RequireRole(OrgBotGlobals.NUMBERS_ROLE_ID)]
        public class CardEditGroup : ModuleBase {

            #region Public Methods

            [Command("discard"), Alias("dontsave", "revert"), Remarks("edit discard")]
            public async Task DiscardChangesAsync() {
                await ReplyAsync("Changes not saved.");
                await YGOCardAPIClient.StopEditingAsync(Context.User, false);
            }

            [Command("text"), Summary("Edits the Name/Flavor Text/Effect/Pendulum Effect of a card"), 
                Remarks("edit text name Kuriboh")]
            public async Task EditEffectAsync([Summary("Data field to change")]CardDataField datafield, 
                [Summary("Value to change data field to"), Remainder] string text) {
                var card = YGOCardAPIClient.GetEditedCard(Context.User);
                switch (datafield) {
                    case CardDataField.Effect:
                    case CardDataField.Description:
                        card.Description = text;
                        break;

                    case CardDataField.PendulumEffect:
                        card.PendulumEffect = text;
                        break;

                    case CardDataField.Name:
                        card.Name = text;
                        break;

                    default:
                        await ReplyAsync("You may only edit the name/effect/flavor text/pendulum effect with this command.");
                        return;
                }
                await ReplyAsync(":thumbsup:");
            }

            [Command("properties"), Alias("prop"), 
                Summary("Edits Attribute/Card Type/Monster Type of a card"), Remarks("edit prop attribute dark")]
            public async Task EditPropertiesAsync([Summary("Data field to change")]CardDataField datafield, 
                [Summary("Value to change data field to")]string value) {
                var card = YGOCardAPIClient.GetEditedCard(Context.User);
                switch (datafield) {
                    case CardDataField.Attribute:
                        card.Attribute = value.ToUpper();
                        break;

                    case CardDataField.CardType:
                        YGOCardType parsedCardType;
                        var parsed = Enum.TryParse(value, out parsedCardType);
                        if (parsed) {
                            card.CardType = parsedCardType;
                        }
                        else {
                            await ReplyAsync("Parsing of Card Type failed.");
                            return;
                        }
                        break;

                    case CardDataField.Type:
                        card.Type = value;
                        break;

                    default:
                        await ReplyAsync("You may only edit properties with this command.");
                        return;
                }
                await ReplyAsync(":thumbsup:");
            }

            [Command("stats"), Summary("Edits the Attack/Defense/Level/Rank/Pendulum Scales of a card"), Remarks("edit stats atk 15000")]
            public async Task EditStatsAsync([Summary("Data field to change")]CardDataField dataField, 
                [Summary("Value to change data field to")]uint value) {
                var card = YGOCardAPIClient.GetEditedCard(Context.User);
                switch (dataField) {
                    case CardDataField.Attack:
                        card.Attack = value;
                        break;

                    case CardDataField.Rank:
                    case CardDataField.Level:
                        card.Level = value;
                        break;

                    case CardDataField.Defense:
                        card.Defence = value;
                        break;

                    case CardDataField.Scales:
                        card.LeftScale = value;
                        card.RightScale = value;
                        break;

                    default:
                        await ReplyAsync("You may only edit numeric stats (ATK/DEF/Level/Rank/Pendulum Scales) with this command.");
                        return;
                }
                await ReplyAsync(":thumbsup:");
            }

            [Command("editing"), Alias("list"), Summary("Lists all the cards being edited and their editors"), Remarks("edit list")]
            public async Task GetCurrentlyBeingEditingAsync() {
                if (!YGOCardAPIClient.IsEditingAny) {
                    await ReplyAsync("No cards are currently being edited.");
                    return;
                }
                await ReplyAsync(string.Join("\n", YGOCardAPIClient.GetEditedCards().Select(x => $"{x.Value.Name} is being edited by {x.Key.Username}")));
            }

            [Command("preview"), 
                Summary("Displays the output of your edits vs the current version."), 
                Remarks("edit preview")]
            public async Task PreviewChangesAsync() {
                var editedCard = YGOCardAPIClient.GetEditedCard(Context.User);
                await ReplyAsync($"Current:\n{YGOCardAPIClient.Cards.GetCard(editedCard.Id).ToDiscordMessage()}\n" +
                                 $"Edited:\n{editedCard.ToDiscordMessage()}");
            }

            [Command("start"), Alias("begin"), Summary("Starts editing a card"), Remarks("edit start Sangan")]
            public async Task StartEditingCardAsync([Summary("Name of card to start editing"), Remainder] string cardName) {
                if (YGOCardAPIClient.IsUserEditing(Context.User)) {
                    await
                        ReplyAsync
                            ("You may only edit one card at a time! Save or discard your changes before editing another card.");
                    return;
                }
                var card = YGOCardAPIClient.Cards.FindCards(cardName).FirstOrDefault();
                if (card.IsBeingEdited()) {
                    var editingUser = YGOCardAPIClient.GetEditor(card);
                    await ReplyAsync($"{card.Name} is currently being edited by {editingUser.Username}.");
                    return;
                }
                YGOCardAPIClient.StartEditing(Context.User, card);
                await ReplyAsync($"{Context.User.Username} has started editing {card.Name}.");
            }

            [Command("save"), Alias("finished", "compete", "done", "commit", "push"), Remarks("edit save")]
            public async Task StopEditingCardAsync() {
                await ReplyAsync("Saving changes to the database!");
                await YGOCardAPIClient.StopEditingAsync(Context.User, true);
            }

            #endregion Public Methods
        }

        #endregion Public Structs + Classes
    }
}