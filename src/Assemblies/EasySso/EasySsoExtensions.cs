using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using static IdentityModel.OidcConstants;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EasySsoExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="loginPath"></param>
        public static void AddEasySsoWithCustomLoginPage(this IServiceCollection services, string loginPath="/Account/Login")
        {
            services.AddIdentity();

            services.AddAuthentication("EasyCookies")
               .AddCookie("EasyCookies");

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Easy.CustomLogin.Cookie";
                config.LoginPath = loginPath;
            });
        }

        public static void AddEasySsoWithCustomLoginPage(this IServiceCollection services, Action<CookieAuthenticationOptions> configureOptions)
        {
            services.AddIdentity();

            services.AddAuthentication("EasyCookies").AddCookie("EasyCookies");

            services.ConfigureApplicationCookie(configureOptions);
        }

        public static void AddEasySso(this IServiceCollection services, string clientId)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "EasyCookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("EasyCookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "EasyCookies";
                options.Authority = "https://localhost:10001";
                options.ClientId = clientId;
                options.ClientSecret = "secret";
                options.ResponseType = ResponseTypes.Code;
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("api");
                options.Scope.Add("offline_access");
            });
        }

        public static void AddEasySso(this IServiceCollection service, Action<OpenIdConnectOptions> configureOptions)
        {
            service.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", configureOptions);
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
