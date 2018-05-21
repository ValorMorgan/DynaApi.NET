using Microsoft.Extensions.Configuration;
using Serilog.Core;

namespace DynaApi.NET.Shared.Factories
{
    public interface ILoggerFactory
    {
        Logger GetLogger();
        Logger GetLoggerFromConfiguration(IConfiguration config);
        void SetupSerilogLogger(IConfiguration configuration);
    }
}