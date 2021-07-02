namespace LogLevelRuntimeSwitching.Models
{
    public class Settings
    {
        public Github Github { get; set; }

        public Logging Logging { get; set; }

        public int CheckIntervalMs { get; set; }
    }
}