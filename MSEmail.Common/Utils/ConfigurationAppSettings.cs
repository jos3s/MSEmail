using Microsoft.Extensions.Configuration;

namespace MSEmail.Common.Utils
{
    public class ConfigurationAppSettings
    {
        private static IConfiguration _configuration;

        private static readonly object _lock = new object();

        public static void ConfigureSettings(IConfiguration configurationSection)
        {
            if (_configuration == null)
            {
                lock (_lock)
                {
                    if (_configuration == null)
                        _configuration = configurationSection;
                }
            }
        }

        public static IConfiguration Configuration { get { return _configuration; } }
    }
}
