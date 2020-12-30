namespace Sitecore.FederatedAuth.SitecoreIdentity
{
    class AppSettings
    {
        public static readonly string SectionName = "Sitecore:ExternalIdentityProviders:IdentityProviders:ADFS";

        public AdfsIdentityProvider ADFSIdentityProvider { get; set; } = new AdfsIdentityProvider();
    }
}
