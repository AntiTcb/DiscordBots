using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LiteDB;

namespace DiscordBCL
{
    public static class LiteDbExtensions
    {
        public static T Get<T>(this LiteRepository rep, BsonValue id)
            => rep.Database.GetCollection<T>().FindById(id);
        public static T GetOne<T>(this LiteRepository rep, Expression<Func<T, bool>> func)
            => rep.Database.GetCollection<T>().FindOne(func);
        public static IEnumerable<T> GetAll<T>(this LiteRepository rep) 
            => rep.Database.GetCollection<T>().FindAll();
        public static bool TryGet<T>(this LiteRepository rep, BsonValue id, out T value)
            => rep.Database.GetCollection<T>().TryGetValue(id, out value);
        public static IEnumerable<T> Where<T>(this LiteRepository rep, Expression<Func<T, bool>> func)
            => rep.Database.GetCollection<T>().Find(func);

        public static bool TryGetValue<T>(this LiteCollection<T> col, BsonValue id, out T value)
        {
            value = col.FindById(id);
            return value != null;
        }
    }
}
