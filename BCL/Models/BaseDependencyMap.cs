using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using BCL.Models.Interfaces;

namespace BCL.Models
{
    public class BaseDependencyMap : DependencyMap
    {
        public BaseDependencyMap(IBotConfig botConfig, Dictionary<ulong, IGuildConfig> guildsConfigs) : base()
        {
            TryAdd(botConfig);
            TryAdd(guildsConfigs);
        }
    }
}
