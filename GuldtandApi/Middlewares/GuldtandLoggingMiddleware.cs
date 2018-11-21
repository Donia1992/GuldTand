using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GuldtandApi.Middlewares
{
    public static class GuldtandLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGuldtandLogger(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GuldtandLoggingMiddleware>();
        }
    }

    public class GuldtandLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public GuldtandLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestBodyText = await GetRequestBodyText(context.Request);

            Console.WriteLine($"======= REQUEST ========");
            Console.WriteLine($"Scheme: {context.Request.Scheme}");
            Console.WriteLine($"Host: {context.Request.Host}");
            Console.WriteLine($"Path: {context.Request.Path}");
            Console.WriteLine($"QueryString: {context.Request.QueryString}");
            Console.WriteLine($"RequestBody: {requestBodyText}");
            Console.WriteLine($"========================");

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                var responseBodyText = await GetResponseBodyText(context.Response);

                Console.WriteLine($"======= RESPONSE =======");
                Console.WriteLine($"StatusCode: {context.Response.StatusCode}");
                Console.WriteLine($"ResponseBody: {responseBodyText}");
                Console.WriteLine($"========================");

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> GetRequestBodyText(HttpRequest request)
        {
            var body = request.Body;

            request.EnableRewind();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var requestBodyText = Encoding.UTF8.GetString(buffer);
            request.Body = body;

            return $"{requestBodyText}";
        }

        private async Task<string> GetResponseBodyText(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string responseBodyText = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{responseBodyText}";
        }
    }
}
