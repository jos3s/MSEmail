using MS.Domain.Enums;
using MSEmail.Common;
using System.ComponentModel.DataAnnotations;

namespace MsEmail.API.Models.EmailModels
{
    public class CreateEmailModel
    {
        [Required(ErrorMessageResourceName = "REQ0001", ErrorMessageResourceType = typeof(APIMsg))]
        [EmailAddress]
        public string EmailFrom { get; set; }

        [Required(ErrorMessageResourceName = "REQ0001", ErrorMessageResourceType = typeof(APIMsg))]
        [EmailAddress]
        public string EmailTo { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }

        [Required(ErrorMessageResourceName = "REQ0001", ErrorMessageResourceType = typeof(APIMsg))]
        public DateTime? SendDate { get; set; }
    }
}
