using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace BackgroundPlaygroundWorker.Logging;

internal static class LoggingConfigurationExtensions
{
    public static void ConfigureLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Async(c => c.Console())
            .WriteTo.Async(c => c.File(new CompactJsonFormatter(), $"logs/{nameof(BackgroundPlaygroundWorker)}.txt", rollingInterval: RollingInterval.Day))
            .CreateLogger();
    }
}