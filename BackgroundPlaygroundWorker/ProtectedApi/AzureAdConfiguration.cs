using System.Globalization;

namespace BackgroundPlaygroundWorker.ProtectedApi
{
    internal sealed record AzureAdConfiguration
    {
        public string Instance { get; init; } = "https://login.microsoftonline.com/{0}";
        public string TenantId { get; init; }
        public string ClientId { get; init; }
        public string Authority => string.Format(CultureInfo.InvariantCulture, Instance, TenantId);
        public string ClientSecret { get; init; }
        public string ApiScope { get; init; }
    }
}
