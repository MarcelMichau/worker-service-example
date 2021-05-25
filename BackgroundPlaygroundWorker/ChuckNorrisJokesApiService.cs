using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundPlaygroundWorker
{
    internal sealed class ChuckNorrisJokesApiService
    {
        private readonly HttpClient _client;

        public ChuckNorrisJokesApiService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.chucknorris.io/");
            _client = client;
        }

        public async Task<ChuckNorrisJokesApiResponse> GetRandomJoke(CancellationToken cancellationToken)
        {
            return await _client.GetFromJsonAsync<ChuckNorrisJokesApiResponse>("jokes/random", cancellationToken);
        }
    }
}
