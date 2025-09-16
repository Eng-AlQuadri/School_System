using System.Text;
using ElmahCore;
using Microsoft.AspNetCore.Http;

namespace Logging.ElmahCore;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Filters out known noise like .well-known/appspecific
            var path = context.Request.Path.ToString();
            if (path.Contains("/.well-known/appspecific/") || path.Contains("/favicon.ico"))
            {
                return;
            }

            string body = "";

            try
            {
                context.Request.EnableBuffering();

                context.Request.Body.Position = 0;

                using var reader = new StreamReader(
                    context.Request.Body, Encoding.UTF8, leaveOpen: true);

                body = await reader.ReadToEndAsync();

                context.Request.Body.Position = 0; // Reset for next component
            }
            catch (Exception readEx)
            {
                body = $"[Unable to read body: {readEx.Message}]";
            }

            var customMessage = ex.Message + $"\n\n[Request Body]:\n" + body;
            
            var wrappedEx = new Exception(customMessage, ex);

            await ElmahExtensions.RaiseError(context, wrappedEx);
        }
    }
}
