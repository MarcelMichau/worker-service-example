using BackgroundPlaygroundWorker.ProtectedApi;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundPlaygroundWorker;

internal sealed class ProtectedApiPollingService(
    ILogger<ProtectedApiPollingService> logger,
    ProtectedApiService protectedApiService)
    : BackgroundService
{
    private readonly ILogger<ProtectedApiPollingService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ProtectedApiService _protectedApiService = protectedApiService ?? throw new ArgumentNullException(nameof(protectedApiService));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Polling for Weather at: {time}", DateTimeOffset.Now);

            try
            {
                var weatherForecasts = await _protectedApiService.GetWeatherForecast(stoppingToken);

                _logger.LogInformation("Weather Forecast - {forecasts}", weatherForecasts);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Whoops, we could not get weather this time!");
            }

            await Task.Delay(10000, stoppingToken);
        }
    }
}