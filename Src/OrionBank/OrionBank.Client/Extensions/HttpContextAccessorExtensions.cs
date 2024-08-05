using System.Security.Claims;

namespace OrionBank.Client.Extensions
{
    internal static class HttpContextAccessorExtensions
    {
        public static string? TryGetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
