// Description:
//
// Solution: DiscordBots
// Project: OrgBot
//
// Created: 10/18/2016 11:18 PM
// Last Revised: 10/18/2016 11:24 PM
// Last Revised by: Alex Gravely

namespace OrgBot.Modules.YGOCard.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    public class CardMap : List<YGOCard>
    {
        public CardMap() { }

        public CardMap(IEnumerable<YGOCard> cards)
        {
            foreach (var card in cards)
            {
                Add(card);
            }
        }

        public IEnumerable<YGOCard> FindCards(string cardName) => this.OrderBy(x => x.Name.Length).Where(x => x.Name.ToLower().Contains(cardName.ToLower()));

        public YGOCard GetCard(uint id) => this.FirstOrDefault(x => x.Id == id);
    }
}