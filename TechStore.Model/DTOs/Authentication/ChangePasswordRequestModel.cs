using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Authentication
{
    public class ChangePasswordRequestModel
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
