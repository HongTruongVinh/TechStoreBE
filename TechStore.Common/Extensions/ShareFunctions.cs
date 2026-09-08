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

        private static long _counter = 0;

        public static string GenerateRandomStringId()
        {
            var now = DateTime.UtcNow;
            var counter = Interlocked.Increment(ref _counter) % 1000;

            return $"{now:yyyyMMddHHmmss}{counter:D3}";
            // return $"{now:yyyyMMddHHmmssfff}{counter:D3}"; // Use milliseconds if you want more precision
        }
    }
}
