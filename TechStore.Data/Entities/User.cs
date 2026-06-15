using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class User : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PasswordHash { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string District { get; set; }

        public int MembershipPoints { get; set; }
        public decimal WalletBalance { get; set; }

        public string? Email { get; set; }
        public string? PictureUrl { get; set; }
        public DateTime? Birthday { get; set; }
        public EGender? Gender { get; set; }

        public required ERole RoleId { get; set; }
        public required EUserStatus Status { get; set; }

        public ICollection<Order>? Orders { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
    }
}
