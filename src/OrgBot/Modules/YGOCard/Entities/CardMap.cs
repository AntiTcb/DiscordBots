#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/18/2016 11:18 PM
// Last Revised: 10/18/2016 11:24 PM
// Last Revised by: Alex Gravely

#endregion

namespace OrgBot.Modules.YGOCard.Entities {
    #region Using

    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class CardMap : List<YGOCard> {
        public CardMap(IEnumerable<YGOCard> cards) {
            foreach (var card in cards) {
                Add(card);
            }
        }

        public YGOCard GetCard(uint id) => this.FirstOrDefault(x => x.Id == id);
        public IEnumerable<YGOCard> FindCards(string cardName) => this.Where(x => x.Name.Contains(cardName));
    }
}