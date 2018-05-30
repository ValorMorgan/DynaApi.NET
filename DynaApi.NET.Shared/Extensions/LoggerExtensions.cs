﻿using System;
using System.Linq;
using Serilog;
using Serilog.Events;

namespace DynaApi.NET.Shared.Extensions
{
    public static class LoggerExtensions
    {
        public static string EVENT_ID_NAME = "EventId";

        public static void LogEvent(this ILogger log, LogEventLevel level, string eventId, string template, params object[] values) =>
            LogEvent(log, level, default, eventId, template, values);

        public static void LogEvent(this ILogger log, LogEventLevel level, Exception ex, string eventId, string template, params object[] values)
        {
            if (log == null || string.IsNullOrWhiteSpace(template))
                return;

            if (eventId == default)
                throw new InvalidOperationException("Event ID required to perform event logging.");

            object[] properties = GetProperties(eventId, values);
            template = SetupTemplate(template);

            if (properties.Any()) {
                if (ex != default(Exception))
                    log.Write(level, ex, template, properties);
                else
                    log.Write(level, template, properties);
            }
            else if (ex != default(Exception))
                log.Write(level, ex, template);
            else
                log.Write(level, template);
        }

        public static void LogEventDebug(this ILogger log, string eventId, string template, params object[] values) =>
            LogEvent(log, LogEventLevel.Debug, eventId, template, values);

        public static void LogEventDebug(this ILogger log, Exception ex, string eventId, string template, params object[] values) =>
            LogEvent(log, LogEventLevel.Debug, ex, eventId, template, values);

        public static void LogEventError(this ILogger log, string eventId, string template, params object[] values) =>
            LogEvent(log, LogEventLevel.Error, eventId, template, values);

        public static void LogEventError(this ILogger log, Exception ex, string eventId, string template, params object[] values) =>
            LogEvent(log, LogEventLevel.Error, ex, eventId, template, values);

        public static void LogEventFatal(this ILogger log, string eventId, string template, params object[] values) =>
            LogEvent(log, LogEventLevel.Fatal, eventId, template, values);

        public static void LogEventFatal(this ILogger log, Exception ex, string eventId, string template, params object[] values) =>
            LogEvent(log, LogEventLevel.Fatal, ex, eventId, template, values);

        public static void LogEventInformation(this ILogger log, string eventId, string template, params object[] values) =>
            LogEvent(log, LogEventLevel.Information, eventId, template, values);

        public static void LogEventInformation(this ILogger log, Exception ex, string eventId, string template, params object[] values) =>
            LogEvent(log, LogEventLevel.Information, ex, eventId, template, values);

        public static void LogEventVerbose(this ILogger log, string eventId, string template, params object[] values) =>
            LogEvent(log, LogEventLevel.Verbose, eventId, template, values);

        public static void LogEventVerbose(this ILogger log, Exception ex, string eventId, string template, params object[] values) =>
            LogEvent(log, LogEventLevel.Verbose, ex, eventId, template, values);

        public static void LogEventWarning(this ILogger log, string eventId, string template, params object[] values) =>
            LogEvent(log, LogEventLevel.Warning, eventId, template, values);

        public static void LogEventWarning(this ILogger log, Exception ex, string eventId, string template, params object[] values) =>
            LogEvent(log, LogEventLevel.Warning, ex, eventId, template, values);

        private static object[] GetProperties(string eventId, object[] values) =>
            new object[] {eventId}
                .Concat(values ?? new object[] { })
                .ToArray();

        private static string SetupTemplate(string template) =>
            !template?.Contains(EVENT_ID_NAME) ?? false ?
                $"<{{{EVENT_ID_NAME}}}> {template.Trim()}" :
                template?.Trim();
    }
}