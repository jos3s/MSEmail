using MsEmail.API.Entities.Common;
using MsEmail.API.Entities.Enums;

namespace MsEmail.API.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; internal set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
