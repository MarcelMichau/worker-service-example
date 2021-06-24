using System.Globalization;

namespace BackgroundPlaygroundWorker.ProtectedApi
{
    internal sealed class AzureAdConfiguration
    {
        public string Instance { get; set; } = "https://login.microsoftonline.com/{0}";
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string Authority => string.Format(CultureInfo.InvariantCulture, Instance, TenantId);
        public string ClientSecret { get; set; }
        public string ApiBaseAddress { get; set; }
        public string ApiScope { get; set; }
    }
}
