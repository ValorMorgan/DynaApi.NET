using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DynaApi.NET.Shared.Constants;
using DynaApi.NET.Shared.Core.Extensions;
using DynaApi.NET.Shared.Core.Middleware;
using DynaApi.NET.Shared.Extensions;
using DynaApi.NET.Shared.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DynaApi.NET.Shared.Core {
    public abstract class StartupBase : IStartup {
        public IContainer ApplicationContainer { get; set; }

        public IConfiguration Configuration { get; }

        protected IHostingEnvironment HostingEnvironment { get; }

        protected StartupBase(IConfiguration configuration, IHostingEnvironment env) {
            var loggerFactory = new LoggerFactory();
            loggerFactory.SetupSerilogLogger(configuration);

            Log.Logger.LogEventVerbose(LoggerEvents.CONSTRUCTOR, LoggerTemplates.CONSTRUCTOR, nameof(StartupBase));

            Configuration = configuration;
            HostingEnvironment = env;
        }

        public void Configure(IApplicationBuilder app) {
            app.ConfigureCommonMiddleware();
            RegisterEvents(app.ApplicationServices.GetService<IApplicationLifetime>());
        }

        public IServiceProvider ConfigureServices(IServiceCollection services) {
            Log.Logger.LogEventVerbose(LoggerEvents.STARTUP, "Adding Services to {Interface}", nameof(IServiceCollection));

            ApplicationContainer = services.BuildApplicationContainer(Configuration);

            return new AutofacServiceProvider(ApplicationContainer);
        }

        protected void ConfigureMvc(ref IApplicationBuilder app)=>
            app.UseMvc();

        protected void ConfigureMvc(ref IApplicationBuilder app, Action<IRouteBuilder> routeBuilder)=>
            app.UseMvc(routeBuilder);

        protected void SetExceptionHandler(ref IApplicationBuilder app)=>
            app.UseDeveloperExceptionPage();

        protected void SetExceptionHandler(ref IApplicationBuilder app, string exceptionRoute)=>
            app.UseExceptionHandler(exceptionRoute);

        public void Dispose() {
            Log.Logger.LogEventDebug(LoggerEvents.DISPOSE, LoggerTemplates.DISPOSING, nameof(StartupBase));

            ApplicationContainer?.Dispose();
            ApplicationContainer = null;
        }

        private void RegisterEvents(IApplicationLifetime applicationLifetime) {
            Log.Logger.LogEventDebug(LoggerEvents.STARTUP, LoggerTemplates.REGISTER_EVENT, nameof(ApplicationContainer), nameof(applicationLifetime.ApplicationStopped));
            applicationLifetime.ApplicationStopped.Register(()=> ApplicationContainer.Dispose());
        }
    }
}