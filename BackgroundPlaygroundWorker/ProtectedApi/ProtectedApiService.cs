using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace BackgroundPlaygroundWorker.ProtectedApi
{
    internal sealed class ProtectedApiService
    {
        private readonly HttpClient _client;
        private readonly AzureAdConfiguration _azureAdConfiguration;
        private readonly IConfidentialClientApplication _confidentialClientApplication;

        public ProtectedApiService(HttpClient client, IOptions<AzureAdConfiguration> options)
        {
            _azureAdConfiguration = options.Value;

            _confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(_azureAdConfiguration.ClientId)
                .WithClientSecret(_azureAdConfiguration.ClientSecret)
                .WithAuthority(new Uri(_azureAdConfiguration.Authority))
                .Build();

            client.BaseAddress = new Uri(_azureAdConfiguration.ApiBaseAddress);

            _client = client;
        }

        public async Task<ProtectedApiResponse> GetWeatherForecast(CancellationToken cancellationToken)
        {
            var authenticationResult = await _confidentialClientApplication
                .AcquireTokenForClient(new List<string> { _azureAdConfiguration.ApiScope })
                .ExecuteAsync(cancellationToken);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authenticationResult.AccessToken);

            return await _client.GetFromJsonAsync<ProtectedApiResponse>("weatherforecast", cancellationToken);
        }
    }
}