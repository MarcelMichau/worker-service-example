using Microsoft.Extensions.DependencyInjection;

namespace BackgroundPlaygroundWorker.ProtectedApi
{
    internal static class ProtectedApiServiceCollectionExtensions
    {
        internal static IServiceCollection ConfigureProtectedApiService(this IServiceCollection services)
        {
            services.AddHttpClient<ProtectedApiService>();

            return services;
        }
    }
}
