using Identity.API;
using Identity.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServicesBuilderExtensions
    {
        public static void AddIdentityInMemory(this IServiceCollection services, IdentityApiConfig identityApiConfig, IIdentityServerBuilder identityServerBuilder)
        {
            services.AddDbContext<CustomIdentityDbContext>(config =>
            {
                config.UseInMemoryDatabase("InMemory");
            });

            AddIdentityInternal(services, identityApiConfig);

            identityServerBuilder.AddAspNetIdentity<IdentityUser>();
        }

        public static void AddIdentityMySql(this IServiceCollection services, IdentityApiConfig identityApiConfig)
        {
            services.AddDbContext<CustomIdentityDbContext>(config =>
            {
                config.UseMySql(identityApiConfig.MySqlConnectionString);
            });
            AddIdentityInternal(services, identityApiConfig);
        }

        private static void AddIdentityInternal(this IServiceCollection services, IdentityApiConfig identityApiConfig)
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
            .AddEntityFrameworkStores<CustomIdentityDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
