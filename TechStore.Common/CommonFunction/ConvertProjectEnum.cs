using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;

namespace TechStore.Common.CommonFunction
{
    public class ConvertProjectEnum
    {
        public static string ConvertRoleIdToName(ERole role)
        {
            if (role == ERole.Admin)
            {
                return AppRoles.Admin;
            }
            else if (role == ERole.Staff)
            {
                return AppRoles.Staff;
            }
            else
            {
                return AppRoles.Customer;
            }
        }
    }
}
