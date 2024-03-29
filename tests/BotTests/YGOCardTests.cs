﻿#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BotTests
// 
// Created: 11/01/2016 5:48 PM
// Last Revised: 11/01/2016 6:56 PM
// Last Revised by: Alex Gravely

#endregion

namespace BotTests {
    #region Using

    using System.Linq;
    using Discord;
    using Discord.WebSocket;
    using Mocks;
    using OrgBot.Modules.YGOCard.Entities;
    using Xunit;

    #endregion

    public class YGOCardTests {
        #region Public Methods

        [Fact]
        public void TestGetNormalMonster() {
            var card = YGOCardAPIClient.Cards.FindCards("sabersaurus").FirstOrDefault();
            var sabersaurus = new YGOCard {
                Attack = 1900,
                Defence = 500,
                Name = "Sabersaurus",
                Attribute = "Earth",
                CardType = YGOCardType.Normal,
                Description =
                    "This normally gentle dinosaur enjoys relaxing in its nest in the prairies. If it becomes angered, it turns terribly ferocious.",
                Id = 3672,
                LeftScale = 0,
                Level = 4,
                PendulumEffect = "",
                RightScale = 0,
                Type = "Dinosaur"
            };
            Assert.Equal(sabersaurus.Attack, card.Attack);
            Assert.Equal(sabersaurus.Defence, card.Defence);
            Assert.Equal(sabersaurus.Name, card.Name);
            Assert.Equal(sabersaurus.Id, card.Id);
            //Assert.Same(sabersaurus, card);
        }

        [Fact]
        public void TestStartEditCard() {
            var card = YGOCardAPIClient.Cards.FindCards("sangan").FirstOrDefault();
            YGOCardAPIClient.StartEditing(Users.PublicUser, card);
            Assert.NotEmpty(YGOCardAPIClient.GetEditedCards());
            Assert.True(card.IsBeingEdited());
            Assert.True(YGOCardAPIClient.IsUserEditing(Users.PublicUser));
        }

        #endregion Public Methods
    }
}