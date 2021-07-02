using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using LogLevelRuntimeSwitching.Models;
using LogLevelRuntimeSwitching.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LogLevelRuntimeSwitching
{
    class Program
    {
        private IConfigurationRoot _config;
        private IConfigurationRoot _loggingConfig;

        private static void Main() => new Program().Start().GetAwaiter().GetResult();

        public async Task Start()
        {
            BuildConfiguration();
            await PrepareInitialConfiguration();
            BuildLoggingConfiguration();

            var provider = ConfigureServices();

            await provider.GetRequiredService<ConfigurationService>().Init();

            await Task.Delay(-1);
        }

        // TODO - This needs to be figured out. Works for now.
        private async Task PrepareInitialConfiguration()
        {
            using var client = new WebClient();

            var repoOwner = _config["Github:RepoOwner"];
            var repo = _config["Github:Repo"];
            var repoBranch = _config["Github:RepoBranch"];
            var loggingConfigFile = _config["Logging:ConfigFile"];

            var json = client.DownloadString(
                new Uri($"https://raw.githubusercontent.com/{repoOwner}/{repo}/{repoBranch}/{loggingConfigFile}"));

            await File.WriteAllTextAsync(loggingConfigFile, json);
        }

        private void BuildConfiguration()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json")
                .Build();
        }

        private void BuildLoggingConfiguration()
        {
            _loggingConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(_config["Logging:ConfigFile"], false, true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_loggingConfig)
                .CreateLogger();
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddOptions()
                .Configure<LoggingSettings>(_loggingConfig)
                .Configure<Settings>(_config)
                .AddSingleton<ConfigurationService>()
                .AddMediatR(typeof(Program));

            return services.BuildServiceProvider();
        }
    }
}
