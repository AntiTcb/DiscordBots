namespace BCL.Extensions {

    using System;

    public static class StringExtensions {

        public static string ToTitleCase(this string str) {
            string[] tokens = str.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i++) {
                string token = tokens[i];
                tokens[i] = token.Substring(0, 1).ToUpper() + token.Substring(1);
            }

            return string.Join(" ", tokens);
        }
    }
}