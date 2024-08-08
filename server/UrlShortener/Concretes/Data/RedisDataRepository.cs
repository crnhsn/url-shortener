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

    public async Task<bool> TryWriteAsync(string key, string value)
    {
        bool writeSucceeded = await _database.StringSetAsync(key, value, when: When.NotExists);
        return writeSucceeded;
    }

    public async Task<ReadResult<string>> TryReadAsync(string key)
    {
        var redisValue = await _database.StringGetAsync(key);
        
        if (redisValue.HasValue)
        {
            return ReadResult<string>.SuccessResult(redisValue.ToString());
        }
        else
        {
            return ReadResult<string>.FailureResult();
        }
    }

    public async Task<bool> KeyExistsAsync(string key)
    {
        return await _database.KeyExistsAsync(key);
    }
}
