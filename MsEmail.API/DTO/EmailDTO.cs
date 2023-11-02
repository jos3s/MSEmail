using MsEmail.API.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MsEmail.API.DTO
{
    public class EmailDTO
    {
        [Required]
        [EmailAddress]
        public string EmailFrom { get; set; }

        [Required]
        [EmailAddress]
        public string EmailTo { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }

        public EmailStatus? Status { get; set; }
    }
}
