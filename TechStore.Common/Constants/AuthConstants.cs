using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Common.Constants
{
    public static class AuthConstants
    {
        public const string ControllerRoute = "authentication";

        public const string AccessTokenCookie = "access_token";
        public const string RefreshTokenCookie = "refresh_token";

        public const string AuthenticationPath = "/api/" + ControllerRoute;
    }
}
