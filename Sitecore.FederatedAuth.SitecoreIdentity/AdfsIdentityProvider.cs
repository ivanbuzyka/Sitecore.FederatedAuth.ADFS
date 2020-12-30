namespace Sitecore.FederatedAuth.SitecoreIdentity
{
    public class AdfsIdentityProvider
    {
        public bool Enabled { get; set; }
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string AuthenticationScheme { get; set; }
        public string MetadataAddress { get; set; }
        public string DisplayName { get; set; }
    }
}
