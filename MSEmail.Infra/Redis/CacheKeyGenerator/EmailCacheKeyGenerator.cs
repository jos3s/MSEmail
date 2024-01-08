using MsEmail.Domain.Entities;
using MSEmail.Common.Providers.CacheKeyProvider;
using MSEmail.Domain.Enums;

namespace MSEmail.Infra.Redis.CacheKeyGenerator;

public class EmailCacheKeyGenerator : CacheKeyGenerator<Email>
{
    public string GenerateKeyByStatus(EmailStatus status)
    {
        return CacheKeyProvider<Email>.New()
            .AddParameter("Status", status.ToString())
            .GenerateKey();
    }

    public string GenerateKeyByStatusAndUserId(EmailStatus status, long userId)
    {
        return CacheKeyProvider<Email>.New()
            .AddParameter("UserId", userId.ToString())
            .AddParameter("Status", status.ToString())
            .GenerateKey();
    }
}
