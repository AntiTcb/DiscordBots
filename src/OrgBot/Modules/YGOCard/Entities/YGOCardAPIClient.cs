// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 10/18/2016 10:57 PM
// Last Revised: 11/05/2016 3:20 PM
// Last Revised by: Alex Gravely

namespace OrgBot.Modules.YGOCard.Entities
{
    using BCL;
    using Discord;
    using RestEase;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class YGOCardAPIClient
    {
        public static CardMap Cards { get; set; }
        public static CardMap EditedCards { get; } = new CardMap();
        public static Dictionary<IUser, uint> EditingCollection { get; } = new Dictionary<IUser, uint>();
        public static bool IsEditingAny => EditingCollection.Count != 0;
        public static readonly IYGOCardAPI API = RestClient.For<IYGOCardAPI>(BASE_URI);

        static YGOCardAPIClient()
        {
            var items = API.GetCardsAsync().GetAwaiter().GetResult();
            Cards = new CardMap(items);
        }

        public static async Task EditCardAsync(YGOCard card)
            => await API.UpdateCardAsync(((OrgBotConfig)Globals.BotConfig).DatabaseToken, card.Id, card);

        public static async Task<YGOCard> GetCardAsync(uint id) => await API.GetCardAsync(id);

        public static async Task<List<YGOCard>> GetCardsAsync() => await API.GetCardsAsync();

        public static YGOCard GetEditedCard(IUser editingUser) => EditedCards.GetCard(EditingCollection[editingUser]);

        public static IEnumerable<KeyValuePair<IUser, YGOCard>> GetEditedCards()
        {
            foreach (var edit in EditingCollection)
            {
                yield return new KeyValuePair<IUser, YGOCard>(edit.Key, EditedCards.GetCard(edit.Value));
            }
        }

        public static IUser GetEditor(YGOCard card) => GetEditor(card.Id);

        public static IUser GetEditor(uint cardID) => EditingCollection.FirstOrDefault(x => x.Value == cardID).Key;

        public static bool IsBeingEdited(this YGOCard card) => EditedCards.Any(x => x.Id == card.Id);

        public static bool IsUserEditing(IUser user) => EditingCollection.Keys.Any(x => x.Id == user.Id);

        public static void StartEditing(IUser callingUser, YGOCard card)
        {
            EditingCollection.Add(callingUser, card.Id);
            EditedCards.Add(new YGOCard(card));
        }

        public static async Task StopEditingAsync(IUser editor, bool postChanges)
        {
            if (postChanges)
            {
                await EditCardAsync(EditedCards.GetCard(EditingCollection[editor]));
            }
            var cardId = EditingCollection[editor];
            EditedCards.Remove(EditedCards.GetCard(EditingCollection[editor]));
            EditingCollection.Remove(editor);
            await Cards.GetCard(cardId).UpdateAsync();
        }

        const string BASE_URI = "https://dncardsapi.apispark.net/v2";
    }
}