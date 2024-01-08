using MSEmail.API.Messages;
using MSEmail.API.Validations;
using System.ComponentModel.DataAnnotations;

namespace MsEmail.API.Models.EmailModel
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

        [DateValid]
        public DateTime? SendDate { get; set; }

        public static implicit operator CreateEmailModel(Domain.Entities.Email email)
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

        public static implicit operator Domain.Entities.Email(CreateEmailModel createEmailModel)
        {
            return new Domain.Entities.Email
            {
                EmailFrom = createEmailModel.EmailFrom,
                EmailTo = createEmailModel.EmailTo,
                Subject = createEmailModel.Subject,
                Body = createEmailModel.Body,
                SendDate = createEmailModel.SendDate,
            };
        }
    }
}
