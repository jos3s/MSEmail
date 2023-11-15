namespace MsEmail.API.Entities.Common
{
    public class ExceptionLog : BaseEntity
    {
        public string Source { get; set; }

        public string Message { get; set; }

        public string? StackTrace { get; set; }
    }
}
