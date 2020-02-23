using Identity.API;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerExtensions
    {
        public static IIdentityServerBuilder AddEasyIdentityServerInMemory(this IServiceCollection services, IdentityApiConfig identityApiConfig)
        {
            return services.AddIdentityServerInternal(identityApiConfig)
           .AddInMemoryApiResources(Config.GetApiResources())
           .AddInMemoryIdentityResources(Config.GetIdentityResources())
           .AddInMemoryClients(Config.GetClients());
        }

        public static IIdentityServerBuilder AddEasyIdentityServer(this IServiceCollection services, IdentityApiConfig identityApiConfig)
        {
            var migrationsAssembly = typeof(IdentityServerExtensions).GetTypeInfo().Assembly.GetName().Name;
            var builder = services.AddIdentityServerInternal(identityApiConfig)
                  .AddConfigurationStore(options =>
                  {
                      options.ConfigureDbContext = b =>
                          b.UseMySql(identityApiConfig.MySql.ConfigurationDbContextConnectionString,
                              sql => sql.MigrationsAssembly(migrationsAssembly));
                  })
                  .AddOperationalStore(options =>
                  {
                      options.ConfigureDbContext = b =>
                         b.UseMySql(identityApiConfig.MySql.PersistedGrantDbContextConnectionString,
                             sql => sql.MigrationsAssembly(migrationsAssembly));
                  })
                  .AddDeveloperSigningCredential(true, "tempkey.rsa");

            return builder;
        }

        private static IIdentityServerBuilder AddIdentityServerInternal(this IServiceCollection services, IdentityApiConfig identityApiConfig)
        {
            var builder = services.AddIdentityServer(x =>
            {
                x.IssuerUri = identityApiConfig.Host;
                x.PublicOrigin = identityApiConfig.Host;
                x.UserInteraction = new UserInteractionOptions()
                {
                    LoginUrl = "/Account/Login",
                    LogoutUrl = "/Account/Logout",
                    ErrorUrl = "/Error"
                };
            })
            .AddDeveloperSigningCredential()
            .AddProfileService<ProfileService>()
            .AddAspNetIdentity<IdentityUser>();

            return builder;
        }
    }
}
