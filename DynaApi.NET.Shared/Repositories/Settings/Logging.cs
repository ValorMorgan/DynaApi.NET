namespace DynaApi.NET.Shared.Repositories.Settings
{
    public class Logging
    {
        public bool IncludeScopes { get; set; }

        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
    }
}