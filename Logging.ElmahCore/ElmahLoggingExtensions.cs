using System;
using System.Text;
using ElmahCore;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.ElmahCore;

public static class ElmahLoggingExtensions
{
    public static IServiceCollection AddElmahLogging(this IServiceCollection services)
    {
        services.AddElmah<XmlFileErrorLog>(options =>
        {
            options.LogPath = "~/logs";
        });

        return services;
    }

    public static IApplicationBuilder UseElmahLogging(this IApplicationBuilder app)
    {
        app.UseWhen(context => context.Request.Path.StartsWithSegments("/elmah"), appBuilder =>
        {
            appBuilder.Use(async (context, next) =>
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                    var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                    var username = decodedUsernamePassword.Split(':')[0];
                    var password = decodedUsernamePassword.Split(':')[1];

                    if (username == "admin" && password == "122333")
                    {
                        await next();
                        return;
                    }
                }

                context.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Response.StatusCode = 401;
            });
        });

        app.UseElmah();

        return app;
    }
}
