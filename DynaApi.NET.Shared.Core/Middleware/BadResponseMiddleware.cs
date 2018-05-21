using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynaApi.NET.Shared.Constants;
using DynaApi.NET.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace DynaApi.NET.Shared.Core.Middleware
{
    /// <summary>
    /// Handler for exceptions from the server being converted to a BadResponse message providing details for the response.
    /// </summary>
    public class BadResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public BadResponseMiddleware(RequestDelegate next)
        {
            Log.Logger.LogEventVerbose(LoggerEvents.CONSTRUCTOR, "Constructing {Class}", nameof(BadResponseMiddleware));

            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // If headers already sent to client or we fail to handle response
                if (context.Response.HasStarted || !await TryHandleBadResponseAsync(context, ex))
                    throw;
            }
        }

        /// <summary>
        /// Converts the provided context and exception to a custom serializable response object.
        /// </summary>
        /// <param name="context">The context where the error occured.</param>
        /// <param name="ex">The exception that occured.</param>
        /// <returns>A custom serializable response object.</returns>
        private BadResponse GetResponse(HttpContext context, Exception ex)
        {
            var response = new BadResponse
            {
                Error = new Error
                {
                    Code = context.Response.StatusCode,
                    Message = ex.Message
                }
            };

            if (ex.InnerException == null)
                return response;

            IList<AdditionalError> errors = new List<AdditionalError>();
            Exception inner = ex.InnerException;

            while (inner != null)
            {
                errors.Add(new AdditionalError
                {
                    Domain = context.Request.Path,
                    Message = inner.Message,
                    Reason = inner.GetType().Name
                });

                inner = inner.InnerException;
            }

            response.Error.Errors = errors.ToArray();
            return response;
        }

        /// <summary>
        /// Serializes the provided serializable response object.
        /// </summary>
        /// <param name="response">The serializable response object.</param>
        /// <returns>A serialized response as a string.</returns>
        private string GetResponseBody(BadResponse response) =>
            JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

        /// <summary>
        /// Attempts to handle the conversion of the exception to a proper response.
        /// </summary>
        /// <param name="context">The context where the error occured.</param>
        /// <param name="ex">The exception that occured.</param>
        /// <returns>True/False if the response was handled and written to the context response body.</returns>
        private async Task<bool> TryHandleBadResponseAsync(HttpContext context, Exception ex)
        {
            try
            {
                Log.Logger.LogEventWarning(LoggerEvents.RESPONSE, "Bad response with status code {Status} being handled.", context.Response.StatusCode);

                // TODO: It may be wiser to let the StatusCode persist instead of forcing it to a 500
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = @"application/json";

                var response = GetResponse(context, ex);
                string body = GetResponseBody(response);

                await context.Response.WriteAsync(body);
            }
            catch
            {
                Log.Logger.LogEventError(LoggerEvents.RESPONSE, "Failed to handle bad response.");

                return false;
            }

            return true;
        }
    }

    class BadResponse
    {
        public Error Error { get; set; }
    }

    class Error
    {
        public int Code { get; set; }

        public AdditionalError[] Errors { get; set; }

        public string Message { get; set; }
    }

    class AdditionalError
    {
        public string Domain { get; set; }

        public string Message { get; set; }

        public string Reason { get; set; }
    }
}