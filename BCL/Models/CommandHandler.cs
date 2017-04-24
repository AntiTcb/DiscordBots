using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using BCL.Models.Interfaces;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace BCL.Models
{
    public class CommandHandler : ICommandHandler
    {                                         
        public IDiscordClient Client { get; }
        public CommandService Service { get; }
        public BaseDependencyMap Map { get; } 
        public Dictionary<ulong, IGuildConfig> GuildConfigs { get; }
        
        public CommandHandler(BaseDependencyMap map, CommandService service = null)
        {              
            Client = map.Get<IDiscordClient>();
            GuildConfigs = Map.Get<Dictionary<ulong, IGuildConfig>>();
            Service = service ?? new CommandService();
        }

        public async virtual Task HandleCommandAsync(SocketMessage msg)
        {
            if (!(msg is SocketUserMessage usrMsg) || usrMsg.Author.IsBot)
                return;

            int argPos = 0;

            bool hasMentionPrefix = usrMsg.HasMentionPrefix(Client.CurrentUser, ref argPos);
            bool hasStringPrefix = usrMsg.HasStringPrefix(prefix, ref argPos);

            if (!hasMentionPrefix || !hasStringPrefix)
                return;

            ICommandContext ctx = null;
            switch (Client)
            {
                case DiscordSocketClient sockClient:
                    ctx = new SocketCommandContext(sockClient, usrMsg);
                    break;

                case DiscordShardedClient shardClient:
                    ctx = new ShardedCommandContext(shardClient, usrMsg);
                    break;

                default:
                    throw new ArgumentException("Client is invalid type.", nameof(Client));
            }

            var result = await Service.ExecuteAsync(ctx ?? throw new ArgumentNullException(nameof(ctx)), argPos, Map).ConfigureAwait(false);
        }

        public async virtual Task InstallAsync()
        {
            await Service.AddModulesAsync(typeof(CommandHandler).GetTypeInfo().Assembly).ConfigureAwait(false);
        }
    }
}
