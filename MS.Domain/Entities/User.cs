﻿using MsEmail.Domain.Entities.Common;

namespace MsEmail.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; internal set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
