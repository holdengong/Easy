using EasySso;
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
    public static class EasyAuthExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="loginPath"></param>
        public static void AddEasyAuthWithCustomLoginPage(this IServiceCollection services, string loginPath=Consts.DefaultLoginPath)
        {
            services.AddIdentity();

            services.AddAuthentication(Consts.CookieSchema)
               .AddCookie(Consts.CookieSchema);

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = Consts.CookieName;
                config.LoginPath = loginPath;
            });
        }

        public static void AddEasyAuthWithCustomLoginPage(this IServiceCollection services, Action<CookieAuthenticationOptions> configureOptions)
        {
            services.AddIdentity();

            services.AddAuthentication(Consts.CookieSchema)
                .AddCookie(Consts.CookieSchema);

            services.ConfigureApplicationCookie(configureOptions);
        }

        public static void AddEasyAuth(this IServiceCollection services, string clientId,string clientSecret,List<string> scopes)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = Consts.CookieSchema;
                options.DefaultChallengeScheme = Consts.OidcSchema;
            })
            .AddCookie(Consts.CookieSchema)
            .AddOpenIdConnect(Consts.OidcSchema, options =>
            {
                options.SignInScheme = Consts.CookieSchema;
                options.Authority = Consts.Authority;
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.ResponseType = ResponseTypes.Code;
                options.SaveTokens = true;
                scopes?.ForEach(_ =>
                {
                    options.Scope.Add(_);
                });
            });
        }

        public static void AddEasyAuth(this IServiceCollection service, Action<OpenIdConnectOptions> configureOptions)
        {
            service.AddAuthentication(options =>
            {
                options.DefaultScheme = Consts.CookieSchema;
                options.DefaultChallengeScheme = Consts.OidcSchema;
            })
            .AddCookie(Consts.CookieSchema)
            .AddOpenIdConnect(Consts.OidcSchema, configureOptions);
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
