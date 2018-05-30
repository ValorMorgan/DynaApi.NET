namespace DynaApi.NET.Shared.Repositories.Settings
{
    public class Serilog
    {
        public string[] Enrich { get; set; }

        public MinimumLevel MinimumLevel { get; set; }

        public WriteTo[] WriteTo { get; set; }
    }

    public class MinimumLevel
    {
        public string Default { get; set; }

        public Override Override { get; set; }
    }

    public class Override
    {
        public string Microsoft { get; set; }

        public string System { get; set; }
    }

    public class WriteTo
    {
        public Args Args { get; set; }

        public string Name { get; set; }
    }

    public class Args
    {
        public string OutputTemplate { get; set; }

        public string PathFormat { get; set; }
    }
}