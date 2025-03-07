namespace RedisExercise.Services
{
	public interface ICacheService
	{
		Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> acquire, TimeSpan? expiration = null);
		Task SetStringAsync<T>(string key, T value, TimeSpan? expiration = null);
		Task RemoveAsync(string key);

	}
}
