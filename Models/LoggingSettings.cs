using System.Collections.Generic;

namespace LogLevelRuntimeSwitching.Models
{
    public class LoggingSettings
    {
        public Serilog Serilog { get; set; }
    }

    public class WriteTo
    {
        public string Name { get; set; }
    }

    public class Args
    {
        public string Policy { get; set; }

        public int? MaximumDestructuringDepth { get; set; }

        public int? MaximumStringLength { get; set; }

        public int? MaximumCollectionCount { get; set; }
    }

    public class Destructure
    {
        public string Name { get; set; }

        public Args Args { get; set; }
    }

    public class Properties
    {
        public string Application { get; set; }
    }

    public class Serilog
    {
        public List<string> Using { get; set; }

        public string MinimumLevel { get; set; }

        public List<WriteTo> WriteTo { get; set; }

        public List<string> Enrich { get; set; }

        public List<Destructure> Destructure { get; set; }

        public Properties Properties { get; set; }
    }
}