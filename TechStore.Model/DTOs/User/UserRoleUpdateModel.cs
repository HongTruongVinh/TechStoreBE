using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.User
{
    public class UserRoleUpdateModel
    {
        public required string UserId { get; set; }
        public required ERole Role { get; set; }
    }
}
