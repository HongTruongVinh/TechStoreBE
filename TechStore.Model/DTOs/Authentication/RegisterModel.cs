using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;
using TechStore.Model.DTOs.User;

namespace TechStore.Model.DTOs.Authentication
{
    public class RegisterModel
    {
        public required string RegisterIdentifier { get; set; }
        public required string Password { get; set; }
        public required ERegisterType RegisterType { get; set; }

        public required UserCreateModel UserInformation { get; set; }
    }
}
