using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TechStore.Common.Models;

namespace TechStore.Common.CommonFunction
{
    public class Validator
    {
        // Validate string: only alpha, numberic, at least one uppercase, at least one numberic
        public static bool IsValidPassword(string input)
        {
            if (input.Length > 18)
            {
                return false;
            }

            // only alpha and numberic
            var onlyAlphaNum = new Regex("^[A-Za-z0-9]+$");
            var hasUppercase = new Regex("[A-Z]");
            var hasDigit = new Regex("[0-9]");

            return onlyAlphaNum.IsMatch(input)
                   && hasUppercase.IsMatch(input)
                   && hasDigit.IsMatch(input);
        }

        // 2. Validate format email
        public static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
            return emailRegex.IsMatch(email);
        }

        // 3. Validate VN phone number
        public static bool IsValidVietnamPhone(string phone)
        {
            // VN phonenumber: Start with 0 or +84, then 3/5/7/8/9 and followed by 8 digits
            var phoneRegex = new Regex(@"^(0|\+84)(3|5|7|8|9)[0-9]{8}$");
            return phoneRegex.IsMatch(phone);
        }

        // Demo test
        //public static void Main(string[] args)
        //{
        //    Console.WriteLine(IsValidString("Hello123"));   // True
        //    Console.WriteLine(IsValidString("hello123"));   // False

        //    Console.WriteLine(IsValidEmail("example@gmail.com"));  // True
        //    Console.WriteLine(IsValidEmail("invalid@@gmail"));     // False

        //    Console.WriteLine(IsValidVietnamPhone("0912345678"));   // True
        //    Console.WriteLine(IsValidVietnamPhone("+84912345678")); // True
        //    Console.WriteLine(IsValidVietnamPhone("0123456789"));   // False
        //}
    }
}
