using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;
using static IdentityModel.OidcConstants;
using static IdentityServer4.Models.IdentityResources;

namespace Identity.API
{
    public static class Config
    {
        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                 new ApiResource("api"),
            };
        }

        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
               new OpenId(),
               new Profile()
            };
        }

        public static List<Client> GetClients()
        {
            var result = new List<Client>();

            result.Add(new Client
            {
                ClientId = "admin",
                ClientName= "admin",
                Description = "Easy框架后台管理系统",
                ClientSecrets = new List<Secret> { new Secret("secret".ToSha256()) },
                AllowedGrantTypes = IdentityServer4.Models.GrantTypes.Code,
                AllowedScopes = new List<string> { "api", StandardScopes.OfflineAccess, StandardScopes.OpenId, StandardScopes.Profile },
                RedirectUris = new List<string> { "https://localhost:20000/signin-oidc" },
                AllowOfflineAccess = true,
                RequireConsent = false,
            });

            result.Add(new Client
            {
                ClientId = "identity",
                ClientName = "identity",
                Description = "IdentityApi",
                ClientSecrets = new List<Secret> { new Secret("secret".ToSha256()) },
                AllowedGrantTypes = IdentityServer4.Models.GrantTypes.Code,
                AllowedScopes = new List<string> { "api", StandardScopes.OfflineAccess, StandardScopes.OpenId, StandardScopes.Profile },
                RedirectUris = new List<string> { "https://localhost:10001/signin-oidc" },
                AllowOfflineAccess = true,
                RequireConsent = false,
            });

            result.Add(new Client
            {
                ClientId = "easyshop_client",
                ClientName = "easyshop_client",
                Description = "网上商城C端",
                ClientSecrets = new List<Secret> { new Secret("secret".ToSha256()) },
                AllowedGrantTypes = IdentityServer4.Models.GrantTypes.ResourceOwnerPassword,
                AllowedScopes = new List<string> { "api", StandardScopes.OfflineAccess, StandardScopes.OpenId, StandardScopes.Profile },
                RedirectUris = new List<string> { "https://localhost:20001/Account/Login" },
                AllowOfflineAccess = true,
                RequireConsent = false,
                 
            });

            return result;
        }
    }
}
