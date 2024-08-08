using StackExchange.Redis;
using UrlShortener.Interfaces;

namespace UrlShortener.Concretes.Data;


public class RedisDataRepository : IDataRepository<string, string>
{
    private readonly IDatabase _database;

    public RedisDataRepository(string redisConnectionString)
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        _database = connectionMultiplexer.GetDatabase();
    }

    public bool TryWrite(string key, string value)
    {
        return _database.StringSet(key, value, when: When.NotExists);
    }

    public bool TryRead(string key, out string value)
    {
        var redisValue = _database.StringGet(key);
        
        if (redisValue.HasValue)
        {
            value = redisValue.ToString();
            return true;
        }
        else
        {
            value = null;
            return false;
        }
    }
}
