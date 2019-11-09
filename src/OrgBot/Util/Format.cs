using System.Text.RegularExpressions;

namespace OrgBot.Util
{
    public static class Format
    {
        const string mediawikiMarkupPattern = @"\[\[(?<NormalText>[^|]+)(?:\|?(?<Vanity>[^|]+)?)\]\]";
        private static readonly Regex markupMatcher = new Regex(mediawikiMarkupPattern, RegexOptions.Compiled);
        private static readonly Regex htmlNewline = new Regex(@"<br\s?/?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string ResolveMarkup(string markup)
        {
            string ReplaceMatch(Match m) => m.Groups["Vanity"].Success ? m.Groups["Vanity"].Value : m.Groups["NormalText"].Value;

            string resolvedMarkdown = markupMatcher.Replace(markup, ReplaceMatch);
            string newlineReplace = htmlNewline.Replace(resolvedMarkdown, "\n");
            return newlineReplace;
        }
    }
}
