using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Service.Interfaces;

namespace TechStore.Service.Implementations
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<User> _passwordHasher = new();

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string password, string hash)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, hash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
