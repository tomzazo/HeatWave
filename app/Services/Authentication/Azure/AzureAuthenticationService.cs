using Azure.Core;
using Azure.Identity;
using HeatWave;
using Microsoft.Extensions.Options;

namespace Services.Authentication.Azure
{
    interface IAzureAuthentication
    {
        public ValueTask<AccessToken> GetAccessTokenAsync();
    }

    class AzureManagedIdentityAuthentication : IAzureAuthentication
    {
        public IConfiguration config { get; set; }

        public AzureManagedIdentityAuthentication(IConfiguration config)
        {
            this.config = config;
        }

        public ValueTask<AccessToken> GetAccessTokenAsync()
        {
            string? scope = this.config.GetValue<string>("TemperatureSource:AzureAPIScope");
            if (scope == null)
            {
                throw new Exception("couldn't read scope from app settings");
            }

            TokenRequestContext requestContext = new TokenRequestContext(new string[] { scope });

            ManagedIdentityCredential creds = new ManagedIdentityCredential();
            return creds.GetTokenAsync(requestContext);
        }
    }

    class AzureEntraAppAuthentication : IAzureAuthentication
    {
        public IConfiguration config { get; set; }

        public AzureEntraAppAuthentication(IConfiguration config)
        {
            this.config = config;
        }

        public ValueTask<AccessToken> GetAccessTokenAsync()
        {
            string? tenantID = this.config.GetValue<string>("OnPremiseInstanceAzureEntraAppCredentials:TenantId");
            string? clientId = this.config.GetValue<string>("OnPremiseInstanceAzureEntraAppCredentials:ClientId");
            string? clientSecret = this.config.GetValue<string>("OnPremiseInstanceAzureEntraAppCredentials:ClientSecret");
            if (tenantID == null || clientId == null | clientSecret == null)
            {
                throw new Exception("couldn't read Azure Entra App credentials from app settings");
            }

            string? scope = this.config.GetValue<string>("TemperatureSource:AzureAPIScope");
            if (scope == null)
            {
                throw new Exception("couldn't read scope from app settings");
            }

            TokenRequestContext requestContext = new TokenRequestContext(new string[] { scope });

            ClientSecretCredential clientSecretCredential = new ClientSecretCredential(tenantID, clientId, clientSecret);
            return clientSecretCredential.GetTokenAsync(requestContext);
        }
    }
}