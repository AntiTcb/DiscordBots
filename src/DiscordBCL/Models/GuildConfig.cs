﻿namespace DiscordBCL.Models
{
    public class GuildConfig
    {
        public ulong Id { get; }
        public string Prefix { get; set; }

        public GuildConfig(ulong id)
            => Id = id;
    }
}