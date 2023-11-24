using MsEmail.API.Models.EmailModel;

namespace MSEmail.API.Models.Email
{
    public class ListEmailModel
    {
        public int Count { get; set; }

        public List<ViewEmailModel> Emails { get; set; }
    }
}
