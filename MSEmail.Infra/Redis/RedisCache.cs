using StackExchange.Redis;
using System.Text.Json;

namespace MSEmail.Infra.Redis;

public class RedisCache
{
    public static IDatabase _db { get; set; } = RedisProvider.Instance.ConfigureDatabase();

    public static T? GetValue<T>(string key)
    {
        var value = _db.StringGet(key);

        if (string.IsNullOrEmpty(value))
            return default(T);
        return JsonSerializer.Deserialize<T>(value!) ?? default(T);
    }

    public static void SetValue<T>(string key, T value, int minutes = 60)
    {
        var entity = JsonSerializer.Serialize(value);

        _db.StringSet(key, entity, TimeSpan.FromMinutes(minutes));
    }

    public static void ClearKey(string key)
    {
        _db.KeyDelete(key);
    }
}

