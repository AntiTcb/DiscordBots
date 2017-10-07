using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DiscordBCL.Configuration;
using LiteDB;

namespace DiscordBCL.Services
{
    public class LiteDbService : IDisposable
    {
        private LiteRepository _dbRepo;

        public LiteDbService(BotConfig config)
            => _dbRepo = new LiteRepository(config.ConnectionString);

        public BsonValue Add<T>(T value)
            => _dbRepo.Insert(value);

        public T Get<T>(BsonValue id)
            => _dbRepo.Get<T>(id);
        public T Get<T>(Expression<Func<T, bool>> func)
            => _dbRepo.GetOne(func);
        public IEnumerable<T> GetAll<T>()
            => _dbRepo.GetAll<T>();
        public bool TryGet<T>(BsonValue id, out T value)
            => _dbRepo.TryGet(id, out value);

        public bool Remove<T>(BsonValue id) 
            => _dbRepo.Delete<T>(id);
        public bool Remove<T>(Expression<Func<T, bool>> func)
            => _dbRepo.Delete(func) > 0;

        public bool Update<T>(T value)
            => _dbRepo.Update(value);
        public IEnumerable<T> Where<T>(Expression<Func<T, bool>> func)
            => _dbRepo.Where(func);

        public void Dispose() => _dbRepo?.Dispose();
    }
}
