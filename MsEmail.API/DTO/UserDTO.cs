using System.ComponentModel.DataAnnotations;

namespace MsEmail.API.DTO
{
    public class UserDTO
    {
        [Required]
        public string Email { get; set; }

        [Required] 
        public string Password { get; set; }

    }
}
