﻿using System;
using System.Collections.Generic;
using DynaApi.NET.Shared.Constants;
using DynaApi.NET.Shared.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DynaApi.NET.Shared.Core
{
    public class WebHost : IDisposable
    {
        private IWebHost _host;

        public WebHost(string[] args, Type startupType)
        {
            try
            {
                _host = GetWebHostBuilder(args, startupType)?.Build();

                if (_host == default(IWebHost))
                    throw new NullReferenceException($"{nameof(IWebHost)} built as a default({nameof(IWebHost)}).");
            }
            catch (Exception ex)
            {
                var wrapper = new ApplicationException($"Failed to build {nameof(IWebHost)} with provided {nameof(args)} and {nameof(startupType)}.", ex);

                // TODO: Log error somehow (global Logger may be null at this point).
                Console.WriteLine(wrapper.ToString());

                throw wrapper;
            }
        }

        // TODO: Don't rely on CreateDefaultBuilder (optimize)
        public static IWebHostBuilder GetWebHostBuilder(string[] args, Type startupType) =>
            Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(GetInMemorySettings());
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddEnvironmentVariables();
                })
                .UseStartup(startupType)
                .UseSerilog();

        // Universal default settings (overridable in .config files)
        public static IEnumerable<KeyValuePair<string, string>> GetInMemorySettings() =>
            new Dictionary<string, string>
            {
                {"Logging:LogLevel:Default", "Warning"},
                {"Serilog:MinimumLevel:Default", "Verbose"},
                {"Serilog:MinimumLevel:Override:Microsoft", "Warning"},
                {"Serilog:MinimumLevel:Override:System", "Warning"},
                {"Serilog:Enrich:0", "FromLogContext"},
                {"Serilog:WriteTo:0", "Console"},
                {"Serilog:WriteTo:1", "Debug"},
                {"Serilog:WriteTo:2:Name", "RollingFile"},
                {"Serilog:WriteTo:2:Args:pathFormat", ".\\Logs\\{Date}.log"},
                {"Serilog:WriteTo:2:Args:outputTemplate", "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}"}
            };

        public int Run()
        {
            int returnCode = 0;

            try
            {
                Log.Logger.LogEventInformation(LoggerEvents.STARTUP, "Starting web host");
                _host.Run();
            }
            catch (Exception ex)
            {
                if (Log.Logger != null)
                    Log.Logger.LogEventFatal(ex, LoggerEvents.SHUTDOWN, "Host terminated unexpectedly");

                returnCode = 1;
            }
            finally
            {
                if (!TryCloseAndFlushLogger())
                    returnCode = 1;
            }

            return returnCode;
        }

        public void Dispose()
        {
            _host?.Dispose();
            _host = null;
        }

        private static bool TryCloseAndFlushLogger()
        {
            if (Log.Logger == null)
                return true;

            try
            {
                Log.Logger.LogEventInformation(LoggerEvents.SHUTDOWN, "Closing and Flushing logger.");
                Log.CloseAndFlush();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}