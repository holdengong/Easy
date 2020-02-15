using EasyAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using static IdentityModel.OidcConstants;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EasyAuthServicesExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="loginPath"></param>
        public static void AddEasyAuthWithCustomLoginPage(this IServiceCollection services, string loginPath=EasyAuthConsts.DefaultLoginPath)
        {
            services.AddHttpClient();

            services.AddIdentity();

            services.AddAuthentication(EasyAuthConsts.CookieSchema)
               .AddCookie(EasyAuthConsts.CookieSchema);

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = EasyAuthConsts.CookieName;
                config.LoginPath = loginPath;
            });
        }

        public static void AddEasyAuthWithCustomLoginPage(this IServiceCollection services, Action<CookieAuthenticationOptions> configureOptions)
        {
            services.AddHttpClient();

            services.AddIdentity();

            services.AddAuthentication(EasyAuthConsts.CookieSchema)
                .AddCookie(EasyAuthConsts.CookieSchema);

            services.ConfigureApplicationCookie(configureOptions);
        }

        public static void AddEasyAuth(this IServiceCollection services, string clientId,string clientSecret)
        {
            services.AddHttpClient();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = EasyAuthConsts.CookieSchema;
                options.DefaultChallengeScheme = EasyAuthConsts.OidcSchema;
            })
            .AddCookie(EasyAuthConsts.CookieSchema)
            .AddOpenIdConnect(EasyAuthConsts.OidcSchema, options =>
            {
                options.SignInScheme = EasyAuthConsts.CookieSchema;
                options.Authority = EasyAuthConsts.Authority;
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.ResponseType = ResponseTypes.Code;
                options.SaveTokens = true;
                
                ProcessClaims(options);
                ProcessScode(options);
            });
        }

        private static void ProcessScode(OpenIdConnectOptions options)
        {
            options.Scope.Clear();
            EasyAuthConsts.DefaultScope?.ForEach(_ =>
            {
                options.Scope.Add(_);
            });
            options.GetClaimsFromUserInfoEndpoint = true;
        }

        private static void ProcessClaims(OpenIdConnectOptions options)
        {
            options.ClaimActions.MapAll();

            //删除不用的claims可以减少cookie体积
            options.ClaimActions.DeleteClaim("amr");
            options.ClaimActions.DeleteClaim("s_hash");
            options.ClaimActions.DeleteClaim("http://schemas.microsoft.com/claims/authnmethodsreferences");
            options.ClaimActions.DeleteClaim("http://schemas.microsoft.com/identity/claims/identityprovider");
            options.ClaimActions.DeleteClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            options.ClaimActions.DeleteClaim("sid");
            options.ClaimActions.DeleteClaim("auth_time");
        }

        public static void AddEasyAuth(this IServiceCollection service, Action<OpenIdConnectOptions> configureOptions)
        {
            service.AddAuthentication(options =>
            {
                options.DefaultScheme = EasyAuthConsts.CookieSchema;
                options.DefaultChallengeScheme = EasyAuthConsts.OidcSchema;
            })
            .AddCookie(EasyAuthConsts.CookieSchema)
            .AddOpenIdConnect(EasyAuthConsts.OidcSchema, configureOptions);
        }

        private static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequiredLength = 1,
                    RequiredUniqueChars = 1,
                    RequireLowercase = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };
            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

            services.AddDbContext<IdentityDbContext>(config =>
            {
                config.UseMySql("Server=localhost;Database=AspNetIdentity;User=root;Password=0402_gyt;");
            });
        }
    }
}
