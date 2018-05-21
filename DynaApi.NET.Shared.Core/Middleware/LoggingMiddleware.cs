using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DynaApi.NET.Shared.Constants;
using DynaApi.NET.Shared.Core.Utilities;
using DynaApi.NET.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace DynaApi.NET.Shared.Core.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            Log.Logger.LogEventVerbose(LoggerEvents.CONSTRUCTOR, "Constructing {Class}", nameof(LoggingMiddleware));

            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            Stopwatch sw = Stopwatch.StartNew();

            await _next(httpContext);
            sw.Stop();

            RequestLogger logger = new RequestLogger();
            logger.LogRequest(httpContext, sw.Elapsed.TotalMilliseconds);
        }
    }
}