using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRouterApi.Middleware
{
    public class UserIdHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "userId" || c.Type == "sub");
                if (userIdClaim != null && !string.IsNullOrEmpty(userIdClaim.Value))
                {
                    context.Request.Headers["X-User-Id"] = userIdClaim.Value;
                }
            }
            await _next(context);
        }
    }
} 