using MSEmail.API.Models.Base;

namespace MSEmail.API.Models.User;

public class ListUserModel : BaseListModel
{
    public List<ViewUserModel> Users { get; set; }
}