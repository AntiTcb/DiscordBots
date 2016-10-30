#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/19/2016 10:34 PM
// Last Revised: 10/19/2016 11:31 PM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot.Modules.YGOCard {
    #region Using

    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Entities;
    using Preconditions;

    #endregion

    public partial class YGOCardModule {
        #region Public Structs + Classes

        [Group("edit"), RequireContext(ContextType.Guild), RequireRole(OrgBotGlobals.NUMBERS_ROLE_ID)]
        public class CardEditGroup : ModuleBase {
            #region Public Methods

            [Command("preview")]
            public async Task PreviewChangesAsync() {
                var editedCard = YGOCardAPIClient.GetEditedCard(Context.User);
                await ReplyAsync($"Current:\n{YGOCardAPIClient.Cards.GetCard(editedCard.Id).ToDiscordMessage()}\n" +
                                 $"Edited:\n{editedCard.ToDiscordMessage()}");
            }

            [Command("properties")]
            public async Task EditPropertiesAsync(CardDataField datafield, string value) {
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
                        break;
                    case CardDataField.Type:
                        break;
                    default:
                        await ReplyAsync("You may only edit properties with this command.");
                        return;
                }
                await ReplyAsync(":thumbsup:");
            }

            [Command("stats")]
            public async Task EditStatsAsync(CardDataField dataField, uint value) {
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
                    case CardDataField.LeftScale:
                    case CardDataField.RightScale:
                        card.LeftScale = value;
                        card.RightScale = value;
                        break;
                    default:
                        await ReplyAsync("You may only edit stats with this command.");
                        return;
                }
                await ReplyAsync(":thumbsup:");
            }

            [Command("text")]
            public async Task EditEffectAsync(CardDataField datafield, [Remainder] string text) {
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
                        await ReplyAsync("You can only edit the name/effect/flavor text/pendulum effect with this command.");
                        return;
                }
                await ReplyAsync(":thumbsup:");
            }

            [Command("editing"), Alias("list")]
            public async Task GetCurrentlyBeingEditingAsync() {
                if (!YGOCardAPIClient.IsEditingAny) {
                    await ReplyAsync("No cards are currently being edited.");
                    return;
                }
                var sb = new StringBuilder();
                foreach (var edit in YGOCardAPIClient.GetEditedCards()) {
                    sb.AppendLine($"{edit.Value.Name} is being edited by {edit.Key.Username}.");
                }
                await ReplyAsync(sb.ToString());
            }

            [Command("start")]
            public async Task StartEditingCardAsync([Remainder] string cardName) {
                var card = YGOCardAPIClient.Cards.FindCards(cardName).FirstOrDefault();
                if (card.IsBeingEdited() || YGOCardAPIClient.IsEditing(Context.User)) {
                    var editingUser = YGOCardAPIClient.GetEditor(card);
                    await ReplyAsync($"{card.Name} is currently being edited by {editingUser.Username}.");
                    return;
                }
                YGOCardAPIClient.StartEditing(Context.User, card);
                await ReplyAsync($"{Context.User.Username} has started editing {card.Name}.");
            }

            [Command("discard"), Alias("dontsave")]
            public async Task DiscardChangesAsync() {
                await ReplyAsync("Changes not saved.");
                await YGOCardAPIClient.StopEditingAsync(Context.User, false);
            }

            [Command("save"), Alias("finished", "compete", "done")]
            public async Task StopEditingCardAsync() {
                await ReplyAsync("Saving changes to the database!");
                await YGOCardAPIClient.StopEditingAsync(Context.User, true);

            }

            #endregion Public Methods
        }

        #endregion Public Structs + Classes
    }
}