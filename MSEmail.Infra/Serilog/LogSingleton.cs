using MSEmail.Common.Utils;
using MsEmail.Domain.Entities.Common;
using MSEmail.Domain.Enums;
using Serilog;
using Serilog.Core;

namespace MSEmail.Infra.Log;
public class LogSingleton
{
    private static LogSingleton _instance { get; set; }
    private static Logger _logger { get; set; }

    private static readonly object _lock = new();

    private LogSingleton()
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File($@"{ConfigHelper.LogPath}\{ConfigHelper.LogFileName}", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public static LogSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new LogSingleton();
                }
            }

            return _instance;
        }
    }

    public void CreateInformationLog(string method, string msg, ServiceType type)
    {
        var obj = new
        {
            method,
            type,
            msg,
            date = DateTime.Now,
        };
        _logger.Information("Info: {obj}", obj);
    }

    public void CreateExceptionLog(ExceptionLog ex)
    {
        var ex1 = new
        {
            ex.Source,
            ex.Message,
            ex.StackTrace,
            ex.ServiceType,
            ex.MethodName,
        };
        _logger.Error("ERROR: {@ex}", ex1);
    }

    public void CreateExceptionLog(Exception ex)
    {
        var ex1 = new
        {
            ex.Source,
            ex.Message,
            ex.StackTrace,
        };
        _logger.Error("ERROR: {@ex}", ex1);
    }
}
