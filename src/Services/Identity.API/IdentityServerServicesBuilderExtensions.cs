using Identity.API;
using IdentityServer4.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerServicesBuilderExtensions
    {
        public static IIdentityServerBuilder AddIdentityServerInMemory(this IServiceCollection services, IdentityApiConfig configuration)
        {
            return services.AddIdentityServerInternal(configuration)
           .AddInMemoryApiResources(Config.GetApiResources())
           .AddInMemoryIdentityResources(Config.GetIdentityResources())
           .AddInMemoryClients(Config.GetClients());
        }

        //public static void AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var migrationsAssembly = typeof(IdentityServerExtensions).GetTypeInfo().Assembly.GetName().Name;
        //    var ssoConfig = configuration.GetSection("IdentityConfig").Get<IdentityConfig>();
        //    var builder = services.AddIdentityServerInternal(configuration)
        //          .AddConfigurationStore(options =>
        //          {
        //              options.ConfigureDbContext = b =>
        //                  b.UseMySql(ssoConfig.MySqlConnectionString,
        //                      sql => sql.MigrationsAssembly(migrationsAssembly));
        //          })
        //            .AddRedisCaching(options =>
        //            {
        //                options.RedisConnectionString = ssoConfig.RedisConnectionString;
        //                options.Db = 15;
        //                options.KeyPrefix = "ConfigurationStore";
        //            })
        //            .AddConfigurationStoreCache()
        //            .AddOperationalStore(options =>
        //            {
        //                options.RedisConnectionString = ssoConfig.RedisConnectionString;
        //                options.Db = 15;
        //                options.KeyPrefix = "OperationalStore";
        //            });
        //    //            builder.AddSigningCredential(new X509Certificate2(
        //    //            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, identityServerOption.CertificateFileName),
        //    //identityServerOption.CertificatePassword));
        //}

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
            .AddDeveloperSigningCredential();

            return builder;
        }
    }
}
