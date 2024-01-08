namespace MSEmail.Infra.Redis.CacheKeyProviders.CacheKeyProvider.Interfaces;

public interface IAddParameter
{
    public IAddParameter AddParameter(string key, string value);
    public string GenerateKey();
}
