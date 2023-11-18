namespace MsEmail.API.Models.EmailModels
{
    public class UpdateEmailModel
    {
        public string? Subject { get; set; }

        public string? Body { get; set; }

        public DateTime? SendDate { get; set; }

        public bool IsNull()
        {
            return Subject.IsNull() && Body.IsNull() && SendDate.IsNull();
        }

    }
}
