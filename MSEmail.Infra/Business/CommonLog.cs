using MsEmail.Domain.Entities.Common;
using MsEmail.Infra.Context;
using MSEmail.Domain.Enums;
using MSEmail.Infra.Repository;

namespace MSEmail.Infra.Business
{
    public class CommonLog
    {
        private AppDbContext _context { get; set; }
        private ExceptionLogRepository _logs { get; set; }

        public CommonLog(AppDbContext context)
        {
            _logs = new ExceptionLogRepository(context);
        }

        public void SaveExceptionLog(Exception ex, string method, string className, ServiceType serviceType)
        {
            try
            {
                ExceptionLog log = new ExceptionLog
                {
                    Source = ex.Source,
                    StackTrace = ex.StackTrace,
                    Message = ex.Message.ToString(),
                    MethodName = method,
                    ClassName = className,
                    ServiceType = serviceType
                };
                _logs.Insert(log).Save();
            }
            catch (Exception)
            {

            }
        }
    }
}
