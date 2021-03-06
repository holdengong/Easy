﻿using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
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
               new Profile(),
               new IdentityResource("userid",new List<string>{ "userid"}),
              new IdentityResource("username",new List<string>{ "username"}),
               new IdentityResource("email",new List<string>{ "email"}),
               new IdentityResource("mobile",new List<string>{ "mobile"}),
            };
        }

        private static List<string> DefaultScope = new List<string>
        {
            "api",
            "openid",
            "profile",
            "userid",
            "username",
            "email",
            "mobile"
        };

        private static List<Secret> DefaultSecret = new List<Secret> { new Secret("secret".ToSha256()) };

    public static List<Client> GetClients()
        {
            var result = new List<Client>();

            result.Add(new Client
            {
                ClientId = "admin",
                ClientName = "admin",
                Description = "Easy框架后台管理系统",
                ClientSecrets = DefaultSecret,
                AllowedGrantTypes = IdentityServer4.Models.GrantTypes.Code,
                AllowedScopes = DefaultScope,
                RedirectUris = new List<string> { "https://localhost:10001/signin-oidc" },
                AllowOfflineAccess = true,
                RequireConsent = false,
                AccessTokenLifetime = 60 * 60 * 24 * 365 
            });

            result.Add(new Client
            {
                ClientId = "easyshop_client",
                ClientName = "easyshop_client",
                Description = "网上商城C端",
                ClientSecrets = DefaultSecret,
                AllowedGrantTypes = IdentityServer4.Models.GrantTypes.ResourceOwnerPassword,
                AllowedScopes = DefaultScope,
                RedirectUris = new List<string> { "https://localhost:10002/Account/Login" },
                AllowOfflineAccess = true,
                RequireConsent = false
            });

            return result;
        }
    }
}
