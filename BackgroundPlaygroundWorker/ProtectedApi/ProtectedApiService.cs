using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundPlaygroundWorker.ProtectedApi
{
    internal sealed class ProtectedApiService
    {
        private readonly HttpClient _client;

        public ProtectedApiService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<List<WeatherForecast>> GetWeatherForecast(CancellationToken cancellationToken)
        {
            return await _client.GetFromJsonAsync<List<WeatherForecast>>("weatherforecast", cancellationToken);
        }
    }
}