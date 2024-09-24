using Microsoft.AspNetCore.Http;
namespace MeesageService.Shared.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetClientIpAddress(this HttpRequest request)
        {
            string? ipAddress = request?.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = request?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
            }

            return ipAddress ?? string.Empty;
        }

        public static string GetBrowserDetails(this HttpRequest request)
        {
            var userAgentString = request?.Headers["User-Agent"].ToString() ?? string.Empty;
            return userAgentString;
        }
    }
}
