namespace MsEmail.API.Entities.Common
{
    public class SystemLog : BaseEntity
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
