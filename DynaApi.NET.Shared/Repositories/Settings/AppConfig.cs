﻿namespace DynaApi.NET.Shared.Repositories.Settings
{
    public class AppConfig
    {
        public ConnectionStrings[] ConnectionStrings { get; set; }

        public Logging Logging { get; set; }

        public Serilog Serilog { get; set; }
    }
}