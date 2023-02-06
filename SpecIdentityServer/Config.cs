using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace SpecIdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified,
                }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("api1", "MyApiScope1"),
            new ApiScope("api2", "MyApiScope2"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "client",
                ClientName = "Client Credentials",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "web",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                // where to redirect after signin
				RedirectUris = { Environment.GetEnvironmentVariable("applicationUrl") + "/signin-oidc" },
                // where to redirect after logout
                FrontChannelLogoutUri = Environment.GetEnvironmentVariable("applicationUrl") + "/signout-oidc",
                PostLogoutRedirectUris = { Environment.GetEnvironmentVariable("applicationUrl") + "/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1",
                    "verification"
                }
            },
        };
}
