using System.Collections.Immutable;
using System.Text.Json;
using DevHands.HighLoadApi.Containers;
using StackExchange.Redis;

namespace DevHands.HighLoadApi.Data;

public class CacheStorage(IConnectionMultiplexer connectionMultiplexer) : IDataStorage
{
    public async Task CreateItems(ImmutableList<DataItem> items)
    {
        var db = connectionMultiplexer.GetDatabase();
        var tasks = items.Select(x =>
                db.StringSetAsync(GetKey(x.Id), JsonSerializer.Serialize(x)))
            .ToList();
        await Task.WhenAll(tasks);
    }

    public async Task<DataItem?> GetItem(long id)
    {
        var db = connectionMultiplexer.GetDatabase();
        var data = await db.StringGetAsync(GetKey(id));

        return data is { HasValue: true, IsNullOrEmpty: false }
            ? JsonSerializer.Deserialize<DataItem>(data.ToString())
            : null;
    }

    private static string GetKey(long id) => $"dh-app-items#x{id}";
}