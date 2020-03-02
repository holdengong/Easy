using Easy.Sso.Assembly;
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
    public static class EasySsoServicesExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="loginPath"></param>
        public static void AddEasyAuthWithCustomLoginPage(this IServiceCollection services, string loginPath=EasySsoConsts.DefaultLoginPath)
        {
            services.AddHttpClient();

            services.AddEasyIdentity();

            services.AddAuthentication(EasySsoConsts.CookieSchema)
               .AddCookie(EasySsoConsts.CookieSchema);

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = EasySsoConsts.CookieName;
                config.LoginPath = loginPath;
            });
        }

        public static void AddEasyAuthWithCustomLoginPage(this IServiceCollection services, Action<CookieAuthenticationOptions> configureOptions)
        {
            services.AddHttpClient();

            services.AddEasyIdentity();

            services.AddAuthentication(EasySsoConsts.CookieSchema)
                .AddCookie(EasySsoConsts.CookieSchema);

            services.ConfigureApplicationCookie(configureOptions);
        }

        public static void AddEasyAuth(this IServiceCollection services, string clientId,string clientSecret)
        {
            services.AddHttpClient();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = EasySsoConsts.CookieSchema;
                options.DefaultChallengeScheme = EasySsoConsts.OidcSchema;
            })
            .AddCookie(EasySsoConsts.CookieSchema)
            .AddOpenIdConnect(EasySsoConsts.OidcSchema, options =>
            {
                options.SignInScheme = EasySsoConsts.CookieSchema;
                options.Authority = EasySsoConsts.Authority;
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.ResponseType = ResponseTypes.Code;
                options.SaveTokens = true;
                options.RequireHttpsMetadata = false;//允许http

                ProcessClaims(options);
                ProcessScode(options);
            });

            services.AddEasyIdentity();
        }

        private static void ProcessScode(OpenIdConnectOptions options)
        {
            options.Scope.Clear();
            EasySsoConsts.DefaultScope?.ForEach(_ =>
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
                options.DefaultScheme = EasySsoConsts.CookieSchema;
                options.DefaultChallengeScheme = EasySsoConsts.OidcSchema;
            })
            .AddCookie(EasySsoConsts.CookieSchema)
            .AddOpenIdConnect(EasySsoConsts.OidcSchema, configureOptions);
        }

        public static void AddEasyIdentity(this IServiceCollection services)
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
            .AddEntityFrameworkStores<EasyIdentityDbContext>()
            .AddDefaultTokenProviders();

            services.AddDbContext<EasyIdentityDbContext>(config =>
            {
                config.UseMySql("Server=148.70.138.124;Database=aspnetidentity;User=root;Password=easy_framework;");
            });
        }
    }
}
