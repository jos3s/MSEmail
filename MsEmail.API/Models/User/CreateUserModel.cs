using System.ComponentModel.DataAnnotations;

namespace MSEmail.API.Models.User;

public class CreateUserModel
{
    private string _email;

    [Required]
    public string Email
    {
        get => _email.ToLower();
        set => _email = value;
    }

    [Required]
    public string Password { get; set; }

    public static implicit operator MsEmail.Domain.Entities.User(CreateUserModel createUser) {
        return new MsEmail.Domain.Entities.User
        {
            Email = createUser.Email,
            Password = createUser.Password.Hashing(),
        };
    }
}