using System.Security.Claims;
using TechStore.Common.Constants;

namespace TechStoreAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetRequiredUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(AppClaims.UserId);

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new InvalidOperationException(
                    "Authenticated user is missing required UserId claim.");
            }

            return userId;
        }
    }
}
