using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TechStore.Common.Extensions
{
    public static class ConvertData
    {
        private static readonly string NumberGroup = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator; // VN "."
        private static readonly string NumberDecimal = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator; // VN ","

        public static decimal ToDecimal(string value)
        {
            // Kiểu My
            if (NumberGroup == "," && NumberDecimal == ".")
            {
                return decimal.Parse(value);
            }

            // Kiểu Việt Nam
            else
            {
                const string temp = "@";
                value = value.Replace(".", temp);
                value = value.Replace(",", ".");
                value = value.Replace(temp, ",");
                return decimal.Parse(value);
            }
        }

        public static string ToStr(decimal pValue)
        {
            string value;

            // kieu My
            if (NumberGroup == "," && NumberDecimal == ".")
            {
                const string temp = "@";

                value = pValue.ToString("#,0.##");
                value = value.Replace(".", temp);
                value = value.Replace(",", ".");
                value = value.Replace(temp, ",");
                return value;
            }

            // Kiểu Việt Nam
            else
            {
                value = pValue.ToString("#,0.##");
                return value;
            }
        }


        public static string ConvertUnicodeToEngLish(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                string temp = value.Normalize(NormalizationForm.FormD);
                string valueId = regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
                return valueId.ToLower().Replace(" ", "-");
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
