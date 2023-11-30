using MsEmail.API.Models.EmailModel;
using MSEmail.API.Models.Base;

namespace MSEmail.API.Models.Email
{
    public class ListEmailModel : BaseListModel
    {
        public List<ViewEmailModel> Emails { get; set; }
    }
}
