namespace WiseOldBot.Modules.OSRS {
    using System;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using RestEase;

    public class AccountDeserializer : IResponseDeserializer {
        #region Implementation of IResponseDeserializer

        public T Deserialize<T>(string content, HttpResponseMessage response)
        {
            var username = response.RequestMessage.RequestUri.ToString().Split(new[] { "player=" }, StringSplitOptions.None)[1];
            var hsType = Regex.Match(response.RequestMessage.RequestUri.ToString(), "(hiscore_oldschool.*?)/").Groups[1].Value;
            var returnAccount = new Account(username, hsType, content.Split('\n'));
            return (T)Convert.ChangeType(returnAccount, typeof(T));
        }

        #endregion
    }
}