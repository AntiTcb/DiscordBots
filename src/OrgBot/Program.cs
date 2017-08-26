using System;
using Discord;

namespace OrgBot
{
    class Program
    {
        static void Main(string[] args)
            => new OrgBot().RunAsync(TokenType.Bot).GetAwaiter().GetResult();
    }
}