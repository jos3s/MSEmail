using System.Text;
using MSEmail.Common.Providers.CacheKeyProvider.Interfaces;

namespace MSEmail.Common.Providers.CacheKeyProvider;

public class CacheKeyProvider<T> :INew, IAddParameter, IGenerateKey
{
    protected Dictionary<string, string> _parameters { get; set; } = new Dictionary<string, string>();

    protected CacheKeyProvider() {}
    public static IAddParameter New() => new CacheKeyProvider<T>();

    public IAddParameter AddParameter(string key, string value)
    {
        if (string.IsNullOrEmpty(value))
            _parameters.Add(key, null);

        if (_parameters.ContainsKey(key))
        {
            _parameters.Remove(key);
        }

        _parameters.Add(key, value);
        return this;
    }

    public string GenerateKey()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append($"{typeof(T)}");

        foreach (KeyValuePair<string, string> values in _parameters)
        {
            sb.Append($"_{values.Key}_{values.Value}");
        }

        return sb.ToString();
    }
}
