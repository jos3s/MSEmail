using MSEmail.Common.Utils;
using StackExchange.Redis;

namespace MSEmail.Infra.Redis;

public class RedisProvider
{
    private static RedisProvider? _redis = null;

    private static readonly object _lock = new();

    public static RedisProvider Instance
    {
        get
        {
            if (_redis == null)
            {
                lock (_lock)
                {
                    _redis ??= new RedisProvider();
                }
            }

            return _redis;
        }
    }

    public IDatabase ConfigureDatabase()
    {
        return ConnectionMultiplexer.Connect
        (
            ConfigHelper.RedisConnection
        ).GetDatabase();
    }
}
