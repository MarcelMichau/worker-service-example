using System;
using System.Collections.Generic;

namespace BackgroundPlaygroundWorker.ProtectedApi
{
    internal sealed record ProtectedApiResponse(List<WeatherForecast> WeatherForecasts);

    internal record WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string Summary { get; set; }
    }
}
