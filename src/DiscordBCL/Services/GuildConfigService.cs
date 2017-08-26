using DiscordBCL.Models;
using System.Collections.Concurrent;

namespace DiscordBCL.Services
{
    public class GuildConfigService
    {
        private readonly LiteDbService _dbService;

        private ConcurrentDictionary<ulong, GuildConfig> _configCache = new ConcurrentDictionary<ulong, GuildConfig>();

        public GuildConfigService(LiteDbService dbService)
            => _dbService = dbService;

        public void AddConfig(ulong guildId, string prefix)
            => _dbService.Add(new GuildConfig(guildId) { Prefix = prefix });

        public GuildConfig GetConfig(ulong guildId)
        {
            if (_configCache.TryGetValue(guildId, out var config))
                return config;
            return _configCache[guildId] = _dbService.Get<GuildConfig>(guildId);
        }

        public void UpdateConfig(GuildConfig config)
        {
            _dbService.Update(config);
            _configCache[config.Id] = config;
        }

        public void RemoveConfig(ulong guildId)
            => _dbService.Remove<GuildConfig>(guildId);
    }
}
