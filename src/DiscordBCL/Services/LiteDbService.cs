using LiteDB;

namespace DiscordBCL.Services
{
    public class LiteDbService
    {
        const string _connString = "Database.db";

        public void Add<T>(T value)
        {
            using (var db = new LiteDatabase(_connString))
            {
                db.GetCollection<T>().Insert(value);
            }
        }

        public T Get<T>(BsonValue id)
        {
            using (var db = new LiteDatabase(_connString))
            {
                return db.GetCollection<T>().FindById(id);
            }
        }

        public bool TryGet<T>(BsonValue id, out T value)
        {
            using (var db = new LiteDatabase(_connString))
            {
                return db.GetCollection<T>().TryGetValue(id, out value);
            }
        }

        public void Remove<T>(BsonValue id)
        {
            using (var db = new LiteDatabase(_connString))
            {
                db.GetCollection<T>().Delete(id);
            }
        }

        public void Update<T>(T value)
        {
            using (var db = new LiteDatabase(_connString))
            {
                db.GetCollection<T>().Update(value);
            }
        }
    }
}
