using MsEmail.Domain.Entities.Common;
using MsEmail.Infra.Context;

using MSEmail.Domain.Enums;
using MSEmail.Infra.Log;
using MSEmail.Infra.Repository;

namespace MSEmail.Infra.Business;

public class CommonLog
{
    private ExceptionLogRepository _logs { get; set; }

    public CommonLog(AppDbContext context) => _logs = new ExceptionLogRepository(context);

    public void SaveExceptionLog(Exception ex, string method, string className, ServiceType serviceType, bool uniqueTransaction = true)
    {
        try
        {
            ExceptionLog log = new ()
            {
                Source = ex.Source,
                StackTrace = ex.StackTrace,
                Message = ex.Message.ToString(),
                MethodName = method,
                ClassName = className,
                ServiceType = serviceType
            };

            LogSingleton.Instance.CreateExceptionLog(log);

            _logs.Insert(log);

            if (uniqueTransaction)
                _logs.Save();
        }
        catch (Exception exInternal) 
        {
            LogSingleton.Instance.CreateExceptionLog(exInternal);
        }
    }
}