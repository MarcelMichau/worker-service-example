using BackgroundPlaygroundWorker.JokesApi;
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
        private readonly JokesApiService _jokesApiService;

        public JokePollingService(ILogger<JokePollingService> logger, JokesApiService jokesApiService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jokesApiService = jokesApiService ?? throw new ArgumentNullException(nameof(jokesApiService));
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
