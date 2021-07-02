using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogLevelRuntimeSwitching.Models;
using MediatR;
using Microsoft.Extensions.Options;
using Octokit;
using Octokit.Internal;
using IRequest = MediatR.IRequest;

namespace LogLevelRuntimeSwitching.Handlers
{
    public class UpdateConfigurationRequest : IRequest { }

    public class UpdateConfigurationHandler : AsyncRequestHandler<UpdateConfigurationRequest>
    {
        private readonly GitHubClient _github;
        private readonly Settings _settings;

        public UpdateConfigurationHandler(IOptions<Settings> settings)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));

            var creds = new Credentials(_settings.Github.Token);
            var credStore = new InMemoryCredentialStore(creds);
            _github = new(new ProductHeaderValue(_settings.Github.ProductName), credStore);
        }

        protected override async Task Handle(UpdateConfigurationRequest request, CancellationToken cancellationToken)
        {
            var test = await _github.Repository.Content.GetRawContent(_settings.Github.RepoOwner, _settings.Github.Repo, _settings.Logging.ConfigFile);
            var json = Encoding.Default.GetString(test);

            await File.WriteAllTextAsync(_settings.Logging.ConfigFile, json, cancellationToken);
        }
    }
}