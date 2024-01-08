using MSEmail.Common.Providers.CacheKeyProvider;
using MsEmail.Domain.Entities.Common;

namespace MSEmail.Infra.Redis.CacheKey;
public class CacheKeyGenerator<T> where T : BaseEntity
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
