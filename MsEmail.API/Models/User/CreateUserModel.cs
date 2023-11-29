using MsEmail.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace MsEmail.API.Models.UserModels
{
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

        public static implicit operator User(CreateUserModel createUser) {
            return new User
            {
                Email = createUser.Email,
                Password = createUser.Password.Hashing(),
            };
        }
    }
}
