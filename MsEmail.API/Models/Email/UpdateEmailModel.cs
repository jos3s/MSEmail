using MSEmail.API.Validations;

namespace MsEmail.API.Models.EmailModel { 
    public class UpdateEmailModel
    {
        public string? Subject { get; set; }

        public string? Body { get; set; }

        [DateValid]
        public DateTime? SendDate { get; set; }

        public bool IsNull()
        {
            return Subject.IsNull() && Body.IsNull() && SendDate.IsNull();
        }

    }
}
