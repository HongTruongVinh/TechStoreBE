using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.User
{
    public class UserUpdateModel
    {
        public required string LastName { get; set; }

        public required string FirstName { get; set; }

        public required string Email { get; set; }

        public DateTime? Birthday { get; set; }

        public EGender? Gender { get; set; }

        public required string Address { get; set; }

        public required string PhoneNumber { get; set; }
    }
}
