

namespace BotTests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using System.Reflection;

    public class MockBot
    {
        public DiscordSocketClient Client { get; set; }
        public CommandService Service { get; set; }
        public DependencyMap Map { get; set; }

        public async Task Run() {

            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Info });
            Service = new CommandService();

            await InstallCommands();

            Client.MessageReceived += HandleCommand;
            //await client.LoginAsync(TokenType.Bot, DiscordBotExtensions.Tokens.LINQPadBot);
            //await client.ConnectAsync();

            //await Task.Delay(-1);
        }

        public async Task InstallCommands() {
            Map = new DependencyMap();
            Map.Add(Client);
            Map.Add(Service);
            await Service.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage parameterMessage) {
            // Don't handle the command if it is a system message
            var message = parameterMessage as SocketUserMessage;
            if (message == null) return;

            // Mark where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message has a valid prefix, adjust argPos 
            if (!(message.HasMentionPrefix(Client.CurrentUser, ref argPos) || message.HasStringPrefix(">>", ref argPos))) return;

            // Create a Command Context
            var context = new CommandContext(Client, message);
            // Execute the Command, store the result
            var result = await Service.ExecuteAsync(context, argPos, Map);

            // If the command failed, notify the user
            if (!result.IsSuccess)
                await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
        }


    }
}
