using Autofac;
using DynaApi.NET.Data.Contexts;
using DynaApi.NET.Data.Factories;
using DynaApi.NET.Data.Mappers;
using DynaApi.NET.Interface.Data;
using DynaApi.NET.Interface.Entity;
using DynaApi.NET.Interface.Model;
using DynaApi.NET.Interface.Service;
using DynaApi.NET.Model;
using DynaApi.NET.Model.Base;
using DynaApi.NET.Model.Mappers;
using DynaApi.NET.Shared.Constants;
using DynaApi.NET.Shared.Extensions;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DynaApi.NET.Service.Utilities
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterLayerTypes(this ContainerBuilder builder, IConfiguration config = null)
        {
            RegisterServiceLayerTypes(ref builder);
            RegisterModelLayerTypes(ref builder);
            RegisterDataLayerTypes(ref builder, config);
            return builder;
        }

        private static void RegisterDataLayerTypes(ref ContainerBuilder builder, IConfiguration config)
        {
            RegisterTypes(ref builder);
            RegisterInstances(ref builder);

            void RegisterTypes(ref ContainerBuilder build)
            {
                Log.Logger.LogEventDebug(LoggerEvents.STARTUP, "Registering {Layer} Layer Types to {Class}", "Data", nameof(ContainerBuilder));
            }

            void RegisterInstances(ref ContainerBuilder build)
            {
                Log.Logger.LogEventDebug(LoggerEvents.STARTUP, "Registering {Layer} Layer Instances to {Class}", "Data", nameof(ContainerBuilder));

                // NOTE: Context are retrieved through the "Mapper"
                // Issues with <out T> type on the factory and with "RegisterInstance" when we want the context to live per request / scope

                build.RegisterGeneric(typeof(EntityDatabaseMapper<>)).As(typeof(IEntityDatabaseMapper<>)).SingleInstance();
            }
        }

        private static void RegisterModelLayerTypes(ref ContainerBuilder builder)
        {
            RegisterTypes(ref builder);
            RegisterInstances(ref builder);

            void RegisterTypes(ref ContainerBuilder build)
            {
                Log.Logger.LogEventDebug(LoggerEvents.STARTUP, "Registering {Layer} Layer Types to {Class}", "Model", nameof(ContainerBuilder));

                build.RegisterType<UserModelMapper>().As<IModelMapper<IUserModel, IUser, IUserProfile>>().SingleInstance();

                build.RegisterGeneric(typeof(EntityRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
                build.RegisterType<UserRepository>().As<IRepository<IUser>>().InstancePerLifetimeScope();
                build.RegisterType<UserProfileRepository>().As<IRepository<IUserProfile>>().InstancePerLifetimeScope();
                build.RegisterType<UserModelRepository>().As<IModelRepository<IUserModel, IUser, IUserProfile>>().InstancePerLifetimeScope();
            }

            void RegisterInstances(ref ContainerBuilder build)
            {
                Log.Logger.LogEventDebug(LoggerEvents.STARTUP, "Registering {Layer} Layer Instances to {Class}", "Model", nameof(ContainerBuilder));
            }
        }

        private static void RegisterServiceLayerTypes(ref ContainerBuilder builder)
        {
            RegisterTypes(ref builder);
            RegisterInstances(ref builder);

            void RegisterTypes(ref ContainerBuilder build)
            {
                Log.Logger.LogEventDebug(LoggerEvents.STARTUP, "Registering {Layer} Layer Types to {Class}", "Service", nameof(ContainerBuilder));
                
                build.RegisterGeneric(typeof(ModelHandler<,>)).As(typeof(IModelHandler<,>)).InstancePerLifetimeScope();
                build.RegisterGeneric(typeof(ModelHandler<,,>)).As(typeof(IModelHandler<,,>)).InstancePerLifetimeScope();
            }

            void RegisterInstances(ref ContainerBuilder build)
            {
                Log.Logger.LogEventDebug(LoggerEvents.STARTUP, "Registering {Layer} Layer Instances to {Class}", "Service", nameof(ContainerBuilder));
            }
        }
    }
}