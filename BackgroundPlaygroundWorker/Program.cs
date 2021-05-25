using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace BackgroundPlaygroundWorker
{
    internal sealed class Program
    {
        private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(3));

        internal static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.Async(c => c.Console())
                .WriteTo.Async(c => c.File(new CompactJsonFormatter(), $"logs/{nameof(BackgroundPlaygroundWorker)}.txt", rollingInterval: RollingInterval.Day))
                .CreateLogger();

            try
            {
                Log.Information("Starting host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((_, services) =>
                {
                    services.AddHostedService<JokePollingService>();
                    services.AddHttpClient<ChuckNorrisJokesApiService>()
                        .AddPolicyHandler(RetryPolicy);
                });
    }
}
