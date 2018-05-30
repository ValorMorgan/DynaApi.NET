﻿using System;
using DynaApi.NET.Shared.Constants;
using DynaApi.NET.Shared.Core;
using DynaApi.NET.Shared.Core.Middleware;
using DynaApi.NET.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DynaApi.NET.API
{
    public class Startup : Shared.Core.StartupBase, IStartup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
            : base(configuration, env)
        {
            Log.Logger.LogEventVerbose(LoggerEvents.CONSTRUCTOR, LoggerTemplates.CONSTRUCTOR, nameof(Startup));
        }

        public new void Configure(IApplicationBuilder app)
        {
            base.Configure(app);

            base.ConfigureMvc(ref app);

            ConfigureForEnvironment(ref app);
        }

        public new IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddControllersAsServices();

            return base.ConfigureServices(services);
        }

        private void ConfigureForEnvironment(ref IApplicationBuilder app)
        {
            Log.Logger.LogEventDebug(LoggerEvents.STARTUP, "Environment: {Environment}", HostingEnvironment.EnvironmentName);
            
            if (HostingEnvironment.IsDevelopment())
                SetupAppForDevelopment(ref app);
            else if (HostingEnvironment.IsProduction())
                SetupAppForProduction(ref app);
        }

        private void SetupAppForDevelopment(ref IApplicationBuilder app)
        {
            SetExceptionHandler(ref app);
        }

        private void SetupAppForProduction(ref IApplicationBuilder app)
        {
            SetExceptionHandler(ref app, "/Home/Error");
        }
    }
}