using MsEmail.API.Entities.Enums;
using MSEmail.Common;
using System.ComponentModel.DataAnnotations;

namespace MsEmail.API.DTO
{
    public class EmailDTO
    {
        [Required (ErrorMessageResourceName = "REQ0001", ErrorMessageResourceType = typeof(APIMsg))]
        [EmailAddress]
        public string EmailFrom { get; set; }

        [Required(ErrorMessageResourceName = "REQ0001", ErrorMessageResourceType = typeof(APIMsg))]
        [EmailAddress]
        public string EmailTo { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }

        public EmailStatus? Status { get; set; }
    }
}
