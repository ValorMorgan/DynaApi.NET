using System;
using System.Collections.Generic;
using System.Linq;
using DynaApi.NET.Shared.Constants;
using DynaApi.NET.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace DynaApi.NET.Shared.Core.Utilities
{
    public class RequestLogger
    {
        private static readonly ILogger _log = Log.ForContext<RequestLogger>();

        public void LogRequest(HttpContext httpContext, double elapsedTime = 0)
        {
            try
            {
                // TODO: Maybe don't assume the response StatusCode needs to be a 400
                int statusCode = httpContext.Response?.StatusCode ?? StatusCodes.Status400BadRequest;

                LogEventLevel level = LogEventLevel.Information;
                if (statusCode >= StatusCodes.Status500InternalServerError)
                    level = LogEventLevel.Error;
                else if (statusCode >= StatusCodes.Status400BadRequest)
                    level = LogEventLevel.Warning;

                var log = _log;
                if (level >= LogEventLevel.Error)
                    TryGetLoggerWithMoreContext(httpContext, out log);

                log.LogEvent(level, LoggerEvents.REQUEST, LoggerTemplates.LOG_WEB_REQUEST, httpContext.Request.Method, httpContext.Request.Path, statusCode, elapsedTime);
            }
            catch (Exception ex)
            {
                LogException(ex, httpContext, elapsedTime);
                throw;
            }
        }

        private static void LogException(Exception ex, HttpContext httpContext, double elapsedTime)
        {
            try
            {
                TryGetLoggerWithMoreContext(httpContext, out var log);

                log.LogEventError(
                    ex,
                    LoggerEvents.REQUEST,
                    LoggerTemplates.LOG_WEB_REQUEST,
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    StatusCodes.Status500InternalServerError,
                    elapsedTime
                );
            }
            catch
            {
                // Atleast try and log the original exception via the logger with less context
                _log.LogEventError(
                    ex,
                    LoggerEvents.REQUEST,
                    LoggerTemplates.LOG_WEB_REQUEST,
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    StatusCodes.Status500InternalServerError,
                    elapsedTime
                );
            }
        }

        private static bool TryGetLoggerWithMoreContext(HttpContext httpContext, out ILogger logger)
        {
            logger = _log;

            try 
            {
                HttpRequest request = httpContext.Request;

                IDictionary<string, string> headers = request.Headers.ToDictionary(
                    h => h.Key,
                    h => h.Value.ToString());

                logger = logger
                    .ForContext("RequestHeaders", headers, destructureObjects: true)
                    .ForContext("RequestHost", request.Host)
                    .ForContext("RequestProtocol", request.Protocol);

                if (!request.HasFormContentType)
                    return true;

                IDictionary<string, string> requestForm = request.Form.ToDictionary(
                    v => v.Key,
                    v => v.Value.ToString());

                logger = logger.ForContext("RequestForm", requestForm);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogEventWarning(ex, LoggerEvents.REQUEST, "Failed to get complete logger context for response {StatusCode}", httpContext.Response.StatusCode);
                return false;
            }
        }
    }
}