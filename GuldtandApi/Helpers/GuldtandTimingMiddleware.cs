using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GuldtandApi.Helpers
{
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

            context.Response.OnStarting(() => {
                sw.Stop();
                Console.WriteLine("===== GULDTAND TIMER =====");
                Console.WriteLine($"Request took {sw.ElapsedMilliseconds} millis.");
                Console.WriteLine("==========================");

                return Task.CompletedTask;
                }
            );

            await _next(context);
        }
    }
}

