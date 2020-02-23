using Identity.API;
using Identity.API.SeedWorks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Easy.Services.Sso
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            ProcessSeedWork(host);
            host.Run();
        }

        /// <summary>
        /// 处理初始化工作
        /// </summary>
        /// <param name="host"></param>
        private static void ProcessSeedWork(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                SeedWork.Initialize(scope.ServiceProvider);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                     webBuilder.UseUrls(GetUrls(args));
                 });

            return builder;
        }

        private static string GetUrls(string[] args)
        {
            return new ConfigurationBuilder()
                            .AddJsonFile("hosting.json")
                            .AddCommandLine(args)
                            .Build()["urls"];
        }
    }
}
