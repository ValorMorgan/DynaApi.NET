using System;
using Microsoft.Extensions.Configuration;

namespace DynaApi.NET.Shared.Factories
{
    public class ConfigurationBuilderFactory : IConfigurationBuilderFactory
    {
        public const string DEVELOPMENT_ENVIRONMENT = "development";
        private const string JSON_EXTENSION = "json";
        private const string SETTING_FILE_NAME = "appsettings";
        
        public IConfigurationBuilder GetBuilder(string basePath = default) => new ConfigurationBuilder()
            .SetBasePath(!string.IsNullOrWhiteSpace(basePath) ? basePath : AppContext.BaseDirectory)
            .AddJsonFile($"{SETTING_FILE_NAME}.{JSON_EXTENSION}", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        public IConfigurationBuilder GetEnvironmentBuilder(string environment, string basePath = default)
        {
            // Null / whitespace returns just the normal Builder
            if (string.IsNullOrWhiteSpace(environment))
                return GetBuilder();

            var builder = GetBuilder(basePath)
                .AddJsonFile($"{SETTING_FILE_NAME}.{environment}.{JSON_EXTENSION}", optional: true);

            if (environment.ToLower().Equals(DEVELOPMENT_ENVIRONMENT.ToLower()))
            {
                // TODO: Development specific configuration
            }

            return builder;
        }
    }
}