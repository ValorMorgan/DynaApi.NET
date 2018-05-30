using System;
using DynaApi.NET.Shared.Constants;
using DynaApi.NET.Shared.Extensions;
using DynaApi.NET.Shared.Core.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace DynaApi.NET.Shared.Core {
    public static class StartupExtensions {
        public static IApplicationBuilder ConfigureCommonMiddleware(this IApplicationBuilder app) {
            Log.Logger.LogEventVerbose(LoggerEvents.STARTUP, "Setting up {Interface} middleware.", nameof(IApplicationBuilder));

            // Logging for requests
            app.UseMiddleware<LoggingMiddleware>();

            // Handles and logs bad responses
            app.UseMiddleware<BadResponseMiddleware>();

            return app;
        }
    }
}