using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundPlaygroundWorker
{
    internal sealed class JokePollingService : BackgroundService
    {
        private readonly ILogger<JokePollingService> _logger;
        private readonly ChuckNorrisJokesApiService _jokesApiService;

        public JokePollingService(ILogger<JokePollingService> logger, ChuckNorrisJokesApiService jokesApiService)
        {
            _logger = logger;
            _jokesApiService = jokesApiService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Polling for new Chuck Norris joke at: {time}", DateTimeOffset.Now);

                try
                {
                    var (id, value) = await _jokesApiService.GetRandomJoke(stoppingToken);

                    _logger.LogInformation("New Joke - Joke ID: {jokeId} - {jokeValue}", id, value);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "Whoops, we could not get a joke this time!");
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
