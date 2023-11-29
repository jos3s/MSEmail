using MsEmail.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace MsEmail.Domain.Entities
{
    public class User : BaseEntity
    {
        private string _email;

        public string Email
        {
            get => _email.ToLower();
            set => _email = value;
        }

        public string Password { get; set; }

        public string Role { get; set; } = "user";
    }
}
