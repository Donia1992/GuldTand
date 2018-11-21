using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GuldtandApi.Middlewares
{
    public static class GuldtandTimingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGuldtandTimer(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GuldtandTimingMiddleware>();
        }
    }

    public class GuldtandTimingMiddleware
    {
        private readonly RequestDelegate _next;

        public GuldtandTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var sw = new Stopwatch();
            sw.Start();

            context.Response.OnStarting(() =>
            {
                sw.Stop();
                context.Response.Headers.Append("ElapsedTime", $"{sw.ElapsedMilliseconds} ms");

                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}

