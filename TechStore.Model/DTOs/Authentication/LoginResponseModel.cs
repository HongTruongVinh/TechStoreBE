using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Model.DTOs.User;

namespace TechStore.Model.DTOs.Authentication
{
    public class LoginResponseModel
    {
        public required UserResponseModel User { get; set; }
        public required string Token { get; set; }
    }
}
