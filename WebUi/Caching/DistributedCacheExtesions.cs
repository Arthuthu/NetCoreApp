using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace WebUi.Caching;

public static class DistributedCacheExtesions
{
	public static async Task SetRecordAsync<T>(this IDistributedCache cache,
		string key,
		T data,
		TimeSpan? absoluteExpireTime = null,
		TimeSpan? unusedExpireTime = null)
	{
		var options = new DistributedCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(1),
			SlidingExpiration = unusedExpireTime
		};

		var value = JsonSerializer.Serialize(data);
		await cache.SetStringAsync(key, value, options);
	}

	public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
	{
		var jsonData = await cache.GetStringAsync(recordId);

		if (jsonData is null)
			return default!;

		return JsonSerializer.Deserialize<T>(jsonData)!;
	}

	public static async Task SetRecordAsync<T>(this IDistributedCache cache,
	string key,
	T data,
	CancellationToken cancellationToken,
	TimeSpan? absoluteExpireTime = null,
	TimeSpan? unusedExpireTime = null)
	{
		var options = new DistributedCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(1),
			SlidingExpiration = unusedExpireTime
		};

		var value = JsonSerializer.Serialize(data);
		await cache.SetStringAsync(key, value, options, cancellationToken);
	}

	public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache,
		string recordId,
		CancellationToken cancellationToken)
	{
		var jsonData = await cache.GetStringAsync(recordId, cancellationToken);

		if (jsonData is null)
			return default!;

		return JsonSerializer.Deserialize<T>(jsonData)!;
	}
}
