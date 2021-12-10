using System;

namespace BackgroundPlaygroundWorker.ProtectedApi;

internal record WeatherForecast
{
    public DateTime Date { get; init; }

    public int TemperatureC { get; init; }

    public int TemperatureF { get; init; }

    public string Summary { get; init; }
}