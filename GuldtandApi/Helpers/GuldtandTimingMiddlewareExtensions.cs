using Microsoft.AspNetCore.Builder;

namespace GuldtandApi.Helpers
{
    public static class GuldtandTimingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGuldtandTimer(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GuldtandTimingMiddleware>();
        }
    }
}
