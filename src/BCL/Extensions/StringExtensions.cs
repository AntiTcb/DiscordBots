#region Header

// Description:
// 
// Solution: DiscordBots
// Project: OrgBot
// 
// Created: 10/18/2016 7:27 PM
// Last Revised: 10/18/2016 7:27 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Extensions {
    #region Using

    using System;

    #endregion

    public static class StringExtensions {
        #region Public Methods

        public static string ToTitleCase(this string str) {
            var tokens = str.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0 ; i < tokens.Length ; i++) {
                var token = tokens[i];
                tokens[i] = token.Substring(0, 1).ToUpper() + token.Substring(1);
            }

            return string.Join(" ", tokens);
        }

        #endregion Public Methods
    }
}