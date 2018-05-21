using Autofac;
using Autofac.Extensions.DependencyInjection;
using DynaApi.NET.Service.Utilities;
using DynaApi.NET.Shared.Constants;
using DynaApi.NET.Shared.Extensions;
using DynaApi.NET.Shared.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DynaApi.NET.Shared.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IContainer BuildApplicationContainer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutofac();

            Log.Logger.LogEventVerbose(LoggerEvents.STARTUP, "Generating {Class}", nameof(ContainerBuilder));
            IContainerBuilderFactory builderFactory = new ContainerBuilderFactory();
            var builder = builderFactory.GetBuilder(configuration);

            Log.Logger.LogEventVerbose(LoggerEvents.STARTUP, "Registering layer types to {Class}", nameof(ContainerBuilder));
            builder.RegisterLayerTypes(configuration);

            Log.Logger.LogEventVerbose(LoggerEvents.STARTUP, "Registering UI Instances to {Class}", nameof(ContainerBuilder));
            builder.RegisterInstance(configuration);

            Log.Logger.LogEventVerbose(LoggerEvents.STARTUP, "Populating services in {Class}", nameof(ContainerBuilder));
            builder.Populate(services);

            return builder.Build();
        }
    }
}