using System.ComponentModel.DataAnnotations;

namespace MsEmail.API.Models.UserModels
{
    public class CreateUserModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
