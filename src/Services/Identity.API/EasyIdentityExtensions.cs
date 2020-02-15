using Identity.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EasyIdentityExtensions
    {
        public static void AddEasyIdentity(this IServiceCollection services, string mysqlConnectionString)
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
                config.UseMySql(mysqlConnectionString);
            });
        }

        public static void AddEasyIdentity(this IServiceCollection services, string mysqlConnectionString, Action<IdentityOptions> setupAction)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(setupAction)
            .AddEntityFrameworkStores<EasyIdentityDbContext>()
            .AddDefaultTokenProviders();

            services.AddDbContext<EasyIdentityDbContext>(config =>
            {
                config.UseMySql(mysqlConnectionString);
            });
        }

        public static void AddEasyIdentityInMemory(this IServiceCollection services)
        {
            services.AddDbContext<EasyIdentityDbContext>(config =>
            {
                config.UseInMemoryDatabase("InMemory");
            });

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
        }
    }
}
