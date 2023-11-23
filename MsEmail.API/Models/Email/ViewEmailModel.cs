using MSEmail.Domain.Enums;

namespace MsEmail.API.Models.EmailModel
{
    public class ViewEmailModel
    {
        public long Id { get; set; }

        public string EmailFrom { get; set; }

        public string EmailTo { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }

        public EmailStatus Status { get; set; }

        public DateTime? SendDate { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime? DeletionDate { get; set; }

        public static implicit operator ViewEmailModel(Domain.Entities.Email email)
        {
            return new ViewEmailModel
            {
                Id = email.Id,
                EmailFrom = email.EmailFrom,
                EmailTo = email.EmailTo,
                Subject = email.Subject,
                Body = email.Body,
                Status = email.Status,
                SendDate = email.SendDate,
                CreationDate = email.CreationDate,
                UpdateDate = email.UpdateDate,
                DeletionDate = email.DeletionDate,
            };
        }
    }
}
