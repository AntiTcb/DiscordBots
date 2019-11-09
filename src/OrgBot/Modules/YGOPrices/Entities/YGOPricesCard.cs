

namespace OrgBot.Modules.YGOPrices.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;

    public struct YGOPricesCard
    {
        public string DataStatus { get; }
        public string PriceStatus { get; }
        public string CardName { get; }
        public string CardText { get; }
        public string CardType { get; }
        public string Type { get; }
        public string Attribute { get; }
        public int? Attack { get; }
        public int? Defense { get; }
        public int? LevelOrRank { get; }
        public string Property { get; }
        public Dictionary<string, CardPriceData> SetData { get; }

        public YGOPricesCard(CardDataResponse data, CardPriceResponse prices)
        {
            DataStatus = data.Status;
            PriceStatus = prices.Status;
            if (data.Status != "success")
            {        
                CardName = data.Status == "fail" 
                    ? "Failed to pull card information." 
                    : $"Status {data.Status}";
                CardText = null;
                CardType = null;
                Type = null;
                Attribute = null;
                Attack = null;
                Defense = null;
                LevelOrRank = null;
                Property = null;
                SetData = null;
                return;
            }  
            CardName = data.CardInfo.Name;
            CardText = data.CardInfo.Text;
            CardType = data.CardInfo.CardType;
            Type = data.CardInfo.Type;
            Attribute = data.CardInfo.Attribute;
            Attack = data.CardInfo.Attack;
            Defense = data.CardInfo.Defense;
            LevelOrRank = data.CardInfo.LevelOrRank;
            Property = data.CardInfo.Property;
            SetData = prices.Sets
                .OrderByDescending(s => s.PriceInfo.Data.Prices.Average)
                .Take(5)
                .ToDictionary(s => $"{s.SetName} | {s.PrintTag} | {s.Rarity}", s => s.PriceInfo);                        
        }

        public EmbedBuilder ToDiscordEmbed()
        {
            var eb = new EmbedBuilder()
               .WithTitle(CardName)
               .WithAuthor((a) => 
                    a.WithName("YugiohPrices.com")
                     .WithIconUrl("http://www.appsgalery.com/pictures/000/629/-ugioh--rices-629524.png")
                     .WithUrl("http://yugiohprices.com"))
               .WithThumbnailUrl("http://yugiohprices.com/img/banner.png");
            if (DataStatus != "success")
            {
                if (DataStatus == "fail")
                {
                    eb.WithDescription("Could not find card information. Double check your input; Card names must be exact.");
                }
                string status = DataStatus;
                eb.AddField((f) => f.WithName("Error Status:").WithValue(status).WithIsInline(true));
            } else
            {
                foreach (var set in SetData)
                {
                    if (set.Value.Status != "success")
                    {
                        eb.AddField((f) => f.WithName(set.Key)
                            .WithValue(set.Value.Message));
                        continue;
                    }
                    eb.AddField((f) => f.WithName(set.Key)
                        .WithValue(set.Value.Data.Prices.ToPriceString()));
                }
            }
            eb.WithCurrentTimestamp()
              .WithFooter((f) =>
                f.WithText("Prices brought to you courtesy of YugiohPrices.com"));
            return eb;
        }
    }
}
