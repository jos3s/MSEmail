using StackExchange.Redis;

namespace MSEmail.Infra.Redis;
public class RedisSingleton
{
    private static IDatabase _redis;

    private static readonly object _lock = new object();

    public static void ConfigureInstance(IDatabase redis)
    {
        if (_redis == null)
        {
            lock (_lock)
            {
                if (_redis == null)
                    _redis = redis;
            }
        }
    }

    public static IDatabase Instance { get { return _redis; } }
}
