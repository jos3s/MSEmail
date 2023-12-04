using Microsoft.Extensions.Configuration;

namespace MSEmail.Common.Utils;

public class ConfigHelper
{
    private static IConfiguration ConfigurationSettings => ConfigurationAppSettings.Configuration;

    public static string GetConfiguration(string key)
    {
        if (ConfigurationSettings.GetSection("ConnectionStrings").GetSection(key) == null)
            return ConfigurationSettings.GetSection("ConnectionStrings").GetSection(key).Value;

        return ConfigurationSettings.GetSection("AppSettings").GetSection(key).Value;
    }

    public static long DefaultUserId
    {
        get
        {
            long value = 0;
            try
            {
                long.TryParse(GetConfiguration("DefaultUserId"), out value);
                return value;
            }
            catch (Exception)
            {
                return value;
            }
        }
    }

    public static string GetSmtpHost
    {
        get
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
    }

    public static int GetSmtpPort
    {
        get
        {
            var value = 0;
            try
            {
                int.TryParse(GetConfiguration("SmtpPort"), out value);
                return value;
            }
            catch (Exception)
            {
                return value;
            }
        }
    }

    public static string GetSmtpUserName
    {
        get
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
    }

    public static string GetSmtpPassword
    {
        get
        {
            try
            {
                return GetConfiguration("SmtpPassword");
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

    public static string GetTokenSecret
    {
        get
        {
            try
            {
                return GetConfiguration("TokenSecret");
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

    public static bool GetRunWorkerCreatedEmail
    {
        get
        {
            var value = false;
            try
            {
                bool.TryParse(GetConfiguration("RunWorkerCreatedEmail"), out value);
                return value;
            }
            catch (Exception)
            {
                return value;
            }
        }
    }

    public static bool GetRunWorkerDraftEmail
    {
        get
        {
            var value = false;
            try
            {
                bool.TryParse(GetConfiguration("RunWorkerDraftEmail"), out value);
                return value;
            }
            catch (Exception)
            {
                return value;
            }
        }
    }

    public static double ServiceInterval
    {
        get
        {
            var value = 10D;
            try
            {
                double.TryParse(GetConfiguration("ServiceInterval"), out value);
                return value;
            }
            catch (Exception)
            {
                return value;
            }
        }
    }
}