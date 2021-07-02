using System;
using System.Threading;
using System.Threading.Tasks;
using LogLevelRuntimeSwitching.Models;
using MediatR;
using Microsoft.Extensions.Options;
using Octokit;
using Octokit.Internal;

namespace LogLevelRuntimeSwitching.Handlers
{
    public class RetrieveShaRequest : IRequest<string> { }
    public class RetrieveShaHandler : IRequestHandler<RetrieveShaRequest, string>
    {
        private readonly GitHubClient _github;
        private readonly Settings _settings;

        public RetrieveShaHandler(IOptions<Settings> settings)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));

            var creds = new Credentials(_settings.Github.Token);
            var credStore = new InMemoryCredentialStore(creds);
            _github = new(new ProductHeaderValue(_settings.Github.ProductName), credStore);
        }

        public async Task<string> Handle(RetrieveShaRequest request, CancellationToken cancellationToken)
        {
            var repo = await _github.Repository.Branch.Get(_settings.Github.RepoOwner, _settings.Github.Repo, _settings.Github.RepoBranch);

            return repo.Commit.Sha;
        }
    }
}