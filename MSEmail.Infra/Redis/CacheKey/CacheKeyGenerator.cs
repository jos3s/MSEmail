using MsEmail.Domain.Entities;

using MSEmail.Infra.Redis.CacheKeyProviders.CacheKeyGenerator;

namespace MSEmail.Infra.Redis.CacheKey;
public class CacheKeyGenerator<T> 
{
    public static string GenerateKeyByUserId(long userId)
    {
        return CacheKeyProvider<T>.New()
            .AddParameter("UserId", userId.ToString())
            .GenerateKey();
    }


    public string GenerateKeyById(long id)
    {
        return CacheKeyProvider<T>.New()
            .AddParameter("Id", id.ToString())
            .GenerateKey();
    }
}
