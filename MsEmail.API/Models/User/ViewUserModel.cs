namespace MSEmail.API.Models.User;

public class ViewUserModel
{
    public long Id { get; set; }

    public string Email { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public DateTime? DeletionDate { get; set; }

    public string Role { get; set; }

    public static implicit operator ViewUserModel(MsEmail.Domain.Entities.User user)
    {
        return new ViewUserModel
        {
            Id = user.Id,
            Email = user.Email,
            CreationDate = user.CreationDate,
            UpdateDate = user.UpdateDate,
            DeletionDate = user.DeletionDate,
            Role = user.Role,
        };
    }
}