using System;
using System.Threading;
using System.Threading.Tasks;
using LogLevelRuntimeSwitching.Handlers;
using LogLevelRuntimeSwitching.Models;
using MediatR;
using Microsoft.Extensions.Options;

namespace LogLevelRuntimeSwitching.Services
{
    public class ConfigurationService : IDisposable
    {
        private string _latestSha;
        private readonly Settings _settings;
        private readonly IMediator _mediator;

        private Timer _processTimer;

        public ConfigurationService(
            IOptions<Settings> settings,
            IMediator mediator)
        {
            _mediator = mediator;
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task Init()
        {
            _latestSha = await _mediator.Send(new RetrieveShaRequest());

            _processTimer = new Timer(async _ =>
            {
                var newSha = await _mediator.Send(new RetrieveShaRequest());

                if (newSha != _latestSha)
                {
                    Console.WriteLine($"New SHA Detected: {newSha}");
                    await _mediator.Send(new UpdateConfigurationRequest());
                    _latestSha = newSha;
                }

                // If we are logging Information, this will display!
                Serilog.Log.Information("Info Log!");
            }, null, 0, _settings.CheckIntervalMs);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _processTimer.Dispose();
            }
        }
    }
}