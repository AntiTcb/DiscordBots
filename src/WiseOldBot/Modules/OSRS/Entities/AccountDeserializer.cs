﻿namespace WiseOldBot.Modules.OSRS.Entities
{
    using BCL.Extensions;
    using RestEase;
    using System;
    using System.Net.Http;

    public class AccountDeserializer : ResponseDeserializer
    {
        public override T Deserialize<T>(string content, HttpResponseMessage response, ResponseDeserializerInfo info)
        {
            var username = response.RequestMessage.RequestUri.ToString().Split(new[] { "player=" }, StringSplitOptions.None)[1];
            var returnAccount = new Account(username.Replace('+', ' ').ToTitleCase(), response.RequestMessage.RequestUri.ToString(), content.Split('\n'));
            return (T)Convert.ChangeType(returnAccount, typeof(T));
        }
    }
}