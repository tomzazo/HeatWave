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
}