using Microsoft.Extensions.Configuration;

namespace DynaApi.NET.Shared.Factories
{
    public interface IConfigurationBuilderFactory
    {
        IConfigurationBuilder GetBuilder(string basePath = default);
        IConfigurationBuilder GetEnvironmentBuilder(string environment, string basePath = default);
    }
}