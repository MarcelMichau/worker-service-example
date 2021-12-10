using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BackgroundPlaygroundWorker.ProtectedApi;

internal static class ProtectedApiServiceCollectionExtensions
{
    internal static IServiceCollection ConfigureProtectedApiService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureAdConfiguration>(configuration.GetSection("AzureAd"));

        services.AddTransient<AuthenticationDelegatingHandler>();

        services.AddHttpClient<ProtectedApiService>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("ProtectedApi:BaseAddress"));
        }).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

        return services;
    }
}