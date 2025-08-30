using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.User
{
    public class UserListItemResponseModel
    {
        public required string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string Name { get => LastName + " " + FirstName; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Address { get; set; }
        public required string PictureUrl { get; set; }

        public required EUserStatus Status { get; set; }
        public required DateTime CreatedAt { get; set; }

    }
}
