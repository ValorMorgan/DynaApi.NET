﻿using Microsoft.Extensions.Configuration;

namespace DynaApi.NET.Shared.Factories
{
    public interface IConfigurationBuilderFactory
    {
        IConfigurationBuilder GetBuilder();
        IConfigurationBuilder GetBuilder(string basePath);
        IConfigurationBuilder GetEnvironmentBuilder(string environment);
        IConfigurationBuilder GetEnvironmentBuilder(string environment, string basePath);
    }
}