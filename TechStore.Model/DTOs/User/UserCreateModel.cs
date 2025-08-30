using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.User
{
    public class UserCreateModel
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? Email { get; set; }

        public required string PasswordHash { get; set; }

        public DateTime? Birthday { get; set; }

        public EGender? Gender { get; set; }

        public required string Address { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
