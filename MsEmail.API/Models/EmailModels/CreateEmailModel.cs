using MS.Domain.Enums;
using MsEmail.Domain.Entities;
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

        public static implicit operator CreateEmailModel(Email email)
        {
            return new CreateEmailModel
            {
                EmailFrom = email.EmailFrom,
                EmailTo = email.EmailTo,
                Subject = email.Subject,
                Body = email.Body,
                SendDate = email.SendDate,
            };
        }

        public static implicit operator Email(CreateEmailModel createEmailModel)
        {
            return new Email
            {
                EmailFrom = createEmailModel.EmailFrom,
                EmailTo = createEmailModel.EmailTo,
                Subject = createEmailModel.Subject,
                Body = createEmailModel.Body,
                SendDate = (DateTime)createEmailModel.SendDate,
            };
        }
    }
}
