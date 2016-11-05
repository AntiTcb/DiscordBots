#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BotTests
// 
// Created: 11/04/2016 4:47 PM
// Last Revised: 11/04/2016 4:47 PM
// Last Revised by: Alex Gravely

#endregion

namespace BotTests.Mocks {
    #region Using

    using System;
    using System.Threading.Tasks;
    using Discord;

    #endregion

    public class MockUser : IUser {
        #region Implementation of IEntity<ulong>

        public ulong Id { get; set; }

        #endregion Implementation of IEntity<ulong>

        #region Implementation of ISnowflakeEntity

        public DateTimeOffset CreatedAt { get; }

        #endregion Implementation of ISnowflakeEntity

        #region Implementation of IMentionable

        public string Mention { get; }

        #endregion Implementation of IMentionable

        #region Implementation of IPresence

        public Game? Game { get; }
        public UserStatus Status { get; }

        #endregion Implementation of IPresence

        #region Implementation of IUser

        public string AvatarId { get; }

        public string AvatarUrl { get; }

        public string Discriminator { get; set; }

        public ushort DiscriminatorValue { get; }

        public bool IsBot { get; }

        public string Username { get; set; }

        public Task<IDMChannel> CreateDMChannelAsync(RequestOptions options = null) {
            return default(Task<IDMChannel>);
        }

        public Task<IDMChannel> GetDMChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null) {
            return default(Task<IDMChannel>);
        }

        #endregion Implementation of IUser
    }
}