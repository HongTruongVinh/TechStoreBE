using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Authentication
{
    public class LoginRequestModel
    {
        public required string LoginIdentifier { get; set; }
        public required string Password { get; set; }
    }
}
