using DiscordBCL.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DiscordBCL.Services
{
    public class GuildConfigService
    {
        private readonly LiteDbService _dbService;
        private ConcurrentDictionary<ulong, GuildConfig> _configCache = new ConcurrentDictionary<ulong, GuildConfig>();

        public GuildConfigService(LiteDbService dbService)
            => _dbService = dbService;

        public void AddConfig(ulong id, string prefix = null)
            => _dbService.Add(new GuildConfig(id) { Prefix = prefix });
        public void AddConfig(GuildConfig config)
            => _dbService.Add(config);

        public GuildConfig GetConfig(ulong guildId)
        {
            if (_configCache.TryGetValue(guildId, out var config))
                return config;
            return _configCache[guildId] = _dbService.Get<GuildConfig>(guildId);
        }

        public IEnumerable<GuildConfig> GetConfigs() 
            => _dbService.GetAll<GuildConfig>();

        public void UpdateConfig(GuildConfig config)
        {
            _dbService.Update(config);
            _configCache[config.Id] = config;
        }

        public void RemoveConfig(ulong guildId)
            => _dbService.Remove<GuildConfig>(guildId);
    }
}
