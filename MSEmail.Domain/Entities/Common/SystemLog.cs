using MSEmail.Domain.Enums;

namespace MsEmail.Domain.Entities.Common
{
    public class SystemLog : BaseEntity
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public ServiceType ServiceType { get; set; }
    }
}
