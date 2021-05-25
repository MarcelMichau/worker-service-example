using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using System;
using System.Net.Http;

namespace BackgroundPlaygroundWorker.JokesApi
{
    internal static class JokesApiServiceCollectionExtensions
    {
        private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(3));

        internal static IServiceCollection ConfigureJokesApiService(this IServiceCollection services)
        {
            services.AddHttpClient<JokesApiService>()
                .AddPolicyHandler(RetryPolicy);

            return services;
        }
    }
}
