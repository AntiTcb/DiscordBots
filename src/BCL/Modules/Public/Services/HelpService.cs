namespace BCL.Modules.Public.Services
{
    using BCL.Comparers;
    using BCL.Extensions;
    using Discord;
    using Discord.Commands;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public static class HelpService
    {
        public static EmbedBuilder GetCommandHelpEmbed(CommandInfo command, SocketCommandContext ctx)
        {
            string prefix = Globals.ServerConfigs[ctx.Guild.Id].CommandPrefix;
            string title = command.Name.ToTitleCase();
            string description = (command.Aliases.Any(a => a != command.Name) ? $"**Aliases: {string.Join(", ", command.Aliases)}**\n\n" : "") +
                $"**__{command.Summary}__**" +
                $"\n\tSignature: `{prefix}{command.Name} {(command.Parameters.Any() ? string.Join(" ", command.Parameters.Select(p => p.Name)) : "")}`" +
                $"\n\tExample: `{prefix}{command.Remarks}`";

            var em = new EmbedBuilder()
                .WithTitle(title)
                .WithDescription(description);

            foreach (var p in command.Parameters)
            {
                if (p.IsOptional)                                                                                                
                    em.AddField(p.Name, $"[{p.Name} = {p.DefaultValue} {(p.IsRemainder ? "...]" : "]")} - Optional {p.Summary}");
                else                                                                               
                    em.AddField(p.Name, $"<{p.Name} {(p.IsRemainder ? "...>" : ">")} {p.Summary}");

                if (p.Type.GetTypeInfo().IsEnum)
                {
                    var enumValues = Enum.GetNames(p.Type).Cast<object>();
                    em.AddField($"Possible values of {p.Name}", string.Join(", ", enumValues.OrderBy(x => x)));
                }
            }
            return em;
        }

        public static string GetCommandHelpString(CommandInfo command)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Join(", ", command.Aliases.Select(x => x)));
            sb.AppendLine($"\tDescription: {command.Summary}\n\tExample: {command.Remarks}");

            foreach (var p in command.Parameters)
            {
                if (p.IsOptional)
                    sb.AppendLine($"[{p.Name} = {p.DefaultValue} {(p.IsRemainder ? "...]" : "]")} - Optional {p.Summary}"); 
                else                                                                                                        
                    sb.AppendLine($"<{p.Name} {(p.IsRemainder ? "...>" : ">")} {p.Summary}");                               

                if (p.Type.GetTypeInfo().IsEnum)
                {
                    var enumValues = Enum.GetNames(p.Type).Cast<object>();
                    sb.AppendLine($"\t\t\tPossible values: {string.Join(", ", enumValues.Select(x => x).OrderBy(x => x))}");
                }
            }
            return sb.ToString();
        }

        public static EmbedBuilder GetGenericHelpEmbed(IEnumerable<ModuleInfo> modules, SocketCommandContext ctx)
        {
            var mods = modules.Where(m => m.CanExecute(ctx));
            string prefix = ctx.Guild == null ? Globals.DEFAULT_PREFIX : Globals.ServerConfigs[ctx.Guild.Id].CommandPrefix;
            var em = new EmbedBuilder()
                .WithTitle("Help")
                .WithDescription("A quick list of all available commands.")
                .WithFooter($"Use {prefix}help <commandname> for specific command information.");
            foreach (var m in mods)
                em.AddField(m.Name, GetModuleString(ctx, m));
            return em;
        }

        public static string GetHelpString(IEnumerable<ModuleInfo> modules)
        {
            var sb = new StringBuilder();
            var listedCommands = new List<string>();
            foreach (var m in modules)
            {
                sb.AppendLine($"{m.Name}");
                foreach (var c in m.Commands)
                {
                    if (listedCommands.Contains(c.Name)) { continue; }
                    listedCommands.Add(c.Name);
                    sb.Append($"\t{string.Join(", ", c.Aliases)} - {c.Summary}\n\t{c.Remarks}");
                }
                sb.AppendLine($"\t{GetHelpString(m.Submodules)}");
            }
            return sb.ToString();
        }

        public static EmbedBuilder GetModuleHelpEmbed(ModuleInfo module, SocketCommandContext ctx)
        {
            string prefix = ctx.Guild == null ? Globals.DEFAULT_PREFIX : Globals.ServerConfigs[ctx.Guild.Id].CommandPrefix;
            var em = new EmbedBuilder()
                .WithTitle(module.Name)
                .WithDescription("Commands:")
                .WithFooter($"Use {prefix}help <commandname> for specific command information.");

            foreach (var c in module.Commands.Distinct(new CommandNameComparer()))   
                em.AddField(string.Join(", ", c.Aliases), $"{Format.Underline(c.Summary ?? "")} - {Format.Italics(c.Remarks ?? "")}"); 

            return em;
        }

        static string GetModuleString(SocketCommandContext ctx, ModuleInfo mod) =>
            string.Join("\n", mod.Commands
                .Distinct(new CommandNameComparer())
                .Where(x => x.CanExecute(ctx))
                .Select(x => $"{string.Join(", ", x.Aliases.Select(a => Format.Underline(a)))} ----- {Format.Italics(x.Summary ?? "No summary available.")}"));
    }
}