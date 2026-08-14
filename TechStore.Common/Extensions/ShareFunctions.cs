using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TechStore.Common.Extensions
{
    public static class ShareFunctions
    {
        public static string ComputeHash<T>(T obj)
        {
            var json = JsonSerializer.Serialize(obj);

            var bytes = Encoding.UTF8.GetBytes(json);

            var hash = SHA256.HashData(bytes);

            return Convert.ToHexString(hash);
        }

        public static string GenarateRandomStringId()
        {
            return $"{DateTime.UtcNow:yyyyMMdd}{(Random.Shared.Next(10000, 100000).ToString() + 1):D6}";
        }
    }
}
