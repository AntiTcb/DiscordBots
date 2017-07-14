using System;                                           
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBCL
{
    public static class PrettyConsole
    {
        private const string date_time_format = "MM/dd hh:mm:ss tt";
        /// <summary> Write a string to the console on an existing line. </summary>
        /// <param name="text">String written to the console.</param>
        /// <param name="foreground">The text color in the console.</param>
        /// <param name="background">The background color in the console.</param>
        public static void Append(string text, ConsoleColor? foreground = null, ConsoleColor? background = null)
        {
            if (foreground == null)
                foreground = ConsoleColor.White;
            if (background == null)
                background = ConsoleColor.Black;

            Console.ForegroundColor = (ConsoleColor)foreground;
            Console.BackgroundColor = (ConsoleColor)background;
            Console.Write(text);
        }
        /// <summary> Write a string to the console on an new line. </summary>
        /// <param name="text">String written to the console.</param>
        /// <param name="foreground">The text color in the console.</param>
        /// <param name="background">The background color in the console.</param>
        public static void WriteLine(string text = "", ConsoleColor? foreground = null, ConsoleColor? background = null)
        {
            if (foreground == null)
                foreground = ConsoleColor.White;
            if (background == null)
                background = ConsoleColor.Black;

            Console.ForegroundColor = (ConsoleColor)foreground;
            Console.BackgroundColor = (ConsoleColor)background;
            Console.Write(Environment.NewLine + text);
        }

        public static void Log(object severity, string source, string message)
        {
            PrettyConsole.WriteLine($"{DateTimeOffset.Now.ToString(date_time_format)} ", ConsoleColor.DarkGray);
            PrettyConsole.Append($"[{severity}] ", GetColorFromSeverity(severity));
            PrettyConsole.Append($"{source}: ", ConsoleColor.DarkGreen);
            PrettyConsole.Append(message, ConsoleColor.White);
        }

        public static Task LogAsync(object severity, string source, string message)
        {
            PrettyConsole.WriteLine($"{DateTimeOffset.Now.ToString(date_time_format)} ", ConsoleColor.DarkGray);
            PrettyConsole.Append($"[{severity}] ", GetColorFromSeverity(severity));
            PrettyConsole.Append($"{source}: ", ConsoleColor.DarkGreen);
            PrettyConsole.Append(message, ConsoleColor.White);
            return Task.CompletedTask;
        }

        public static void Log(IUserMessage msg)
        {
            var channel = (msg.Channel as IGuildChannel);
            PrettyConsole.WriteLine($"{DateTimeOffset.Now.ToString(date_time_format)} ", ConsoleColor.Gray);

            if (channel?.Guild == null)
                PrettyConsole.Append($"[PM] ", ConsoleColor.Magenta);
            else
                PrettyConsole.Append($"[{channel.Guild.Name} #{channel.Name}] ", ConsoleColor.DarkGreen);

            PrettyConsole.Append($"{msg.Author}: ", ConsoleColor.Green);
            PrettyConsole.Append(msg.Content, ConsoleColor.White);
        }

        public static void Log(ICommandContext c)
        {
            var channel = (c.Channel as SocketGuildChannel);
            PrettyConsole.WriteLine($"{DateTimeOffset.Now.ToString(date_time_format)} ", ConsoleColor.Gray);

            if (channel == null)
                PrettyConsole.Append($"[PM] ", ConsoleColor.Magenta);
            else
                PrettyConsole.Append($"[{c.Guild.Name} #{channel.Name}] ", ConsoleColor.DarkGreen);

            PrettyConsole.Append($"{c.User}: ", ConsoleColor.Green);
            PrettyConsole.Append(c.Message.Content, ConsoleColor.White);
        }

        private static ConsoleColor GetColorFromSeverity(object severity)
        {
            if (severity is LogSeverity sev)
            {
                switch (sev)
                {
                    case LogSeverity.Critical:
                        return ConsoleColor.DarkRed;
                    case LogSeverity.Error:
                        return ConsoleColor.Red;
                    case LogSeverity.Warning:
                        return ConsoleColor.Yellow;
                    case LogSeverity.Info:
                        return ConsoleColor.Cyan;
                    case LogSeverity.Verbose:
                        return ConsoleColor.DarkCyan;
                    case LogSeverity.Debug:
                        return ConsoleColor.DarkYellow;
                    default:
                        return ConsoleColor.White;
                }
            }
            return ConsoleColor.Red;
        }
    }
}
