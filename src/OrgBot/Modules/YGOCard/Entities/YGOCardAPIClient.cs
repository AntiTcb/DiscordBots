#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/18/2016 10:57 PM
// Last Revised: 10/20/2016 5:29 PM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot.Modules.YGOCard.Entities {
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BCL;
    using Discord;
    using RestEase;

    #endregion

    public static class YGOCardAPIClient {
        #region Public Fields + Properties

        public static CardMap Cards { get; set; }
        public static bool IsEditingAny => EditingCollection.Count != 0;
        static CardMap EditedCards { get; set; } = new CardMap();
        static Dictionary<IUser, uint> EditingCollection { get; } = new Dictionary<IUser, uint>();
        static readonly IYGOCardAPI API = RestClient.For<IYGOCardAPI>(BASE_URI);

        #endregion Public Fields + Properties

        #region Private Fields + Properties

        const string BASE_URI = "https://dncardsapi.apispark.net/v2";

        #endregion Private Fields + Properties

        #region Public Constructors

        static YGOCardAPIClient() {
            var items = API.GetCardsAsync().Result;
            Cards = new CardMap(items);
        }

        public static async Task<YGOCard> GetCardAsync(uint id) => await API.GetCardAsync(id);

        public static async Task<List<YGOCard>> GetCardsAsync() => await API.GetCardsAsync();

        public static async Task EditCardAsync(YGOCard card)
            => await API.UpdateCardAsync(((OrgBotConfig) Globals.BotConfig).DatabaseToken, card.Id, card);

        public static YGOCard GetEditedCard(IUser editingUser) => EditedCards.GetCard(EditingCollection[editingUser]);

        public static IEnumerable<KeyValuePair<IUser, YGOCard>> GetEditedCards() {
            foreach (var edit in EditingCollection) {
                yield return new KeyValuePair<IUser, YGOCard>(edit.Key, EditedCards.GetCard(edit.Value));
            }
        }

        public static IUser GetEditor(YGOCard card) => GetEditor(card.Id);

        public static IUser GetEditor(uint cardID) => EditingCollection.FirstOrDefault(x => x.Value == cardID).Key;

        public static bool IsBeingEdited(this YGOCard card) => EditedCards.Contains(card);
        public static bool IsEditing(IUser user) => EditingCollection.ContainsKey(user);

        public static void StartEditing(IUser callingUser, YGOCard card) {
            EditingCollection.Add(callingUser, card.Id);
            EditedCards.Add(card);
        }

        public static async Task StopEditingAsync(IUser editor, bool postChanges) {
            if (postChanges) {
                await EditCardAsync(EditedCards.GetCard(EditingCollection[editor]));
            }
            var cardId = EditingCollection[editor];
            EditedCards.Remove(EditedCards.GetCard(EditingCollection[editor]));
            EditingCollection.Remove(editor);
            await Cards.GetCard(cardId).UpdateAsync();
        }

        #endregion Public Constructors
    }
}