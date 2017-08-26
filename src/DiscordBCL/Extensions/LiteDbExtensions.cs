using LiteDB;

namespace DiscordBCL
{
    public static class LiteDbExtensions
    {
        public static bool TryGetValue<T>(this LiteCollection<T> col, BsonValue id, out T value)
        {
            value = col.FindById(id);
            return value != null;
        }
    }
}
