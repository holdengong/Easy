using Identity.API;
using Identity.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServicesBuilderExtensions
    {
        public static void AddEasyIdentityInMemory(this IServiceCollection services, IdentityApiConfig identityApiConfig, IIdentityServerBuilder identityServerBuilder)
        {
            services.AddDbContext<EasyIdentityDbContext>(config =>
            {
                config.UseInMemoryDatabase("InMemory");
            });

            AddIdentityInternal(services, identityServerBuilder);
        }

        public static void AddEasyIdentityMySql(this IServiceCollection services, IdentityApiConfig identityApiConfig, IIdentityServerBuilder identityServerBuilder)
        {
            services.AddDbContext<EasyIdentityDbContext>(config =>
            {
                config.UseMySql(identityApiConfig.MySql.IdentityDbContextConnectionString);
            });
            AddIdentityInternal(services, identityServerBuilder);
        }

        private static void AddIdentityInternal(this IServiceCollection services, IIdentityServerBuilder identityServerBuilder)
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

            identityServerBuilder.AddAspNetIdentity<IdentityUser>();
        }
    }
}
