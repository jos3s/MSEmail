using MSEmail.Common.Utils;

using StackExchange.Redis;

namespace MSEmail.Infra.Redis;
public class RedisConfiguration
{
    public IDatabase Database()
    {
        return ConnectionMultiplexer.Connect
        (
            BuildConfigurations()
        ).GetDatabase();
    }

    public ConfigurationOptions BuildConfigurations()
    {
        return new ConfigurationOptions
        {
            EndPoints = { ConfigHelper.RedisConnection },
            AbortOnConnectFail = false,
            Password = ConfigHelper.RedisPassword
        };
    }
}
