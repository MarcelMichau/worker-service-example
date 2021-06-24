using BackgroundPlaygroundWorker.JokesApi;
using BackgroundPlaygroundWorker.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using BackgroundPlaygroundWorker.ProtectedApi;

namespace BackgroundPlaygroundWorker
{
    internal sealed class Program
    {
        internal static int Main(string[] args)
        {
            LoggingConfigurationExtensions.ConfigureLogging();

            try
            {
                Log.Information("Starting Worker Service...");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Worker Service terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    //services.AddHostedService<JokePollingService>();
                    //services.ConfigureJokesApiService();

                    services.AddHostedService<ProtectedApiPollingService>();
                    services.ConfigureProtectedApiService();
                    services.Configure<AzureAdConfiguration>(hostBuilderContext.Configuration.GetSection("AzureAd"));
                });
    }
}
