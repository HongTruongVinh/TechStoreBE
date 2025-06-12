using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TechStore.Common.CommonFunction
{
    public class CommonFuntion
    {
        public static string GenerateSlug(string phrase)
        {
            string str = phrase.ToLowerInvariant();

            // Bỏ dấu tiếng Việt
            str = RemoveDiacritics(str);

            // Bỏ ký tự đặc biệt
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            // Thay khoảng trắng bằng dấu -
            str = Regex.Replace(str, @"\s+", "-").Trim('-');

            return str;
        }

        public static string RemoveDiacritics(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static List<string> FormartListTags(string input)
        {
            List<string> tags = new List<string>();

            input = input.Replace("#", "");

            // Tách các từ, bỏ qua khoảng trắng dư
            string[] words = Regex.Split(input, @"\s+")
                                  .Where(word => !string.IsNullOrWhiteSpace(word))
                                  .ToArray();

            foreach (string word in words)
            {
                tags.Add(word);
            }

            return tags;
        }
    }
}
