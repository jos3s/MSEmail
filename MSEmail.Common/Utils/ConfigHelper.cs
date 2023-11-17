using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEmail.Common.Utils
{
    public class ConfigHelper
    {
        private static IConfiguration ConfigurationSettings => ConfigurationAppSettings.Configuration;

        public static string GetConfiguration(string key)
        {
            if (ConfigurationSettings.GetSection("ConnectionStrings").GetSection(key) == null)
                return ConfigurationSettings.GetSection("ConnectionStrings").GetSection(key).Value;

            return ConfigurationSettings.GetSection("AppSettings").GetSection(key).Value;
        }

        public static long DefaultUserId()
        {
            try
            {
                long value = 0;
                long.TryParse(GetConfiguration("DefaultUserId"), out value);
                return value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string GetSmtpHost()
        {
            try
            {
                return GetConfiguration("SmtpHost");
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static int GetSmtpPort()
        {
            try
            {
                int value = 0;
                int.TryParse(GetConfiguration("SmptPort"), out value);
                return value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string GetSmtpUserName()
        {
            try
            {
                return GetConfiguration("SmtpUserName");
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetSmtpPassword()
        {
            try
            {
                return GetConfiguration("SmtpPassword");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetTokenSecret()
        {
            try
            {
                return GetConfiguration("TokenSecret");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
