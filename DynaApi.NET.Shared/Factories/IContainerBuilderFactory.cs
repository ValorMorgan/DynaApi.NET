using Autofac;
using Microsoft.Extensions.Configuration;

namespace DynaApi.NET.Shared.Factories
{
    public interface IContainerBuilderFactory
    {
        ContainerBuilder GetBuilder(IConfiguration config = null);
    }
}