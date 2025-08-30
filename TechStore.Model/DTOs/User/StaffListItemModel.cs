using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.User
{
    public class StaffListItemModel
    {
        public required string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string Name { get => LastName + " " + FirstName; }

        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }

        public DateTime? Birthday { get; set; }
        public EGender? Gender { get; set; }
        public required string PictureUrl { get; set; }

        public required EUserStatus Status { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}
