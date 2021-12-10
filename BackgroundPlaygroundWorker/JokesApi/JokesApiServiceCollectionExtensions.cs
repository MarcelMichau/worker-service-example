using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using System;
using System.Net.Http;

namespace BackgroundPlaygroundWorker.JokesApi;

internal static class JokesApiServiceCollectionExtensions
{
    private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy = HttpPolicyExtensions
        .HandleTransientHttpError()
        .Or<TimeoutRejectedException>()
        .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(3));

    internal static IServiceCollection ConfigureJokesApiService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<JokesApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("JokesApi:BaseAddress"));
            })
            .AddPolicyHandler(RetryPolicy);

        return services;
    }
}