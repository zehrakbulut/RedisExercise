using Newtonsoft.Json;
using RedisExercise.Services;
using StackExchange.Redis;

public class RedisCacheService : ICacheService
{
	private readonly IDatabase _redisDb;

	public RedisCacheService(IConnectionMultiplexer redis)
	{
		_redisDb = redis.GetDatabase();
	}

	public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> acquire, TimeSpan? expiration = null)
	{
		try
		{
			var cachedData = await _redisDb.StringGetAsync(key);

			if (!cachedData.IsNullOrEmpty)
			{
				return JsonConvert.DeserializeObject<T>(cachedData);
			}

			T data = await acquire();

			if (data != null)
			{
				await SetStringAsync(key, data, expiration);
			}

			return data;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Cache error: {ex.Message}");
			throw;
		}
	}

	public async Task SetStringAsync<T>(string key, T value, TimeSpan? expiration = null)
	{
		var serializedValue = JsonConvert.SerializeObject(value);
		await _redisDb.StringSetAsync(key, serializedValue, expiration);
	}

	public async Task RemoveAsync(string key)
	{
		await _redisDb.KeyDeleteAsync(key);
	}
}