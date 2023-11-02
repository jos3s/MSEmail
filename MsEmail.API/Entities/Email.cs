using MsEmail.API.Entities.Enums;

namespace MsEmail.API.Entities
{
    public class Email : BaseEntity
    {
        public string EmailFrom { get; set; }
        
        public string EmailTo { get; set; }
        
        public string? Subject { get; set; }

        public string? Body { get; set; }

        public EmailStatus Status { get; set; }
    }
}
