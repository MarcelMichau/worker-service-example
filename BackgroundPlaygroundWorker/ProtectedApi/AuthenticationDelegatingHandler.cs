using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundPlaygroundWorker.ProtectedApi
{
    internal class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly AzureAdConfiguration _azureAdConfiguration;
        private readonly IConfidentialClientApplication _confidentialClientApplication;
        private readonly ILogger<AuthenticationDelegatingHandler> _logger;

        public AuthenticationDelegatingHandler(IOptions<AzureAdConfiguration> options, ILogger<AuthenticationDelegatingHandler> logger)
        {
            _azureAdConfiguration = options.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(_azureAdConfiguration.ClientId)
                .WithClientSecret(_azureAdConfiguration.ClientSecret)
                .WithAuthority(new Uri(_azureAdConfiguration.Authority))
                .Build();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await GetToken(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
            _logger.LogDebug("Starting HttpRequest...");
            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }

        public async Task<AuthenticationResult> GetToken(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("Starting Token HttpRequest...");

                var authenticationResult = await _confidentialClientApplication
                    .AcquireTokenForClient(new List<string> { _azureAdConfiguration.ApiScope })
                    .ExecuteAsync(cancellationToken);

                _logger.LogDebug("Completed Token HttpRequest");

                return authenticationResult;
            }
            catch (MsalUiRequiredException ex)
            {
                // The application doesn't have sufficient permissions.
                // - Did you declare enough app permissions during app creation?
                // - Did the tenant admin grant permissions to the application?

                _logger.LogError(ex, "The application doesn't have sufficient permissions");
                throw;
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be in the form "https://resourceurl/.default"
                // Mitigation: Change the scope to be as expected.

                _logger.LogError(ex, "An invalid scope was requested");
                throw;
            }
        }
    }
}
