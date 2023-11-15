using MsEmail.API.Entities.Common;
using MsEmail.API.Entities.Enums;
using System.Text;

namespace MsEmail.API.Entities
{
    public class Email : BaseEntity
    {
        public string EmailFrom { get; set; }
        
        public string EmailTo { get; set; }
        
        public string? Subject { get; set; }

        public string? Body { get; set; }

        public EmailStatus Status { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"EmailFrom :{EmailFrom} - ");
            stringBuilder.Append($"EmailTo :{EmailTo}");
            return stringBuilder.ToString();
        }
    }
}
