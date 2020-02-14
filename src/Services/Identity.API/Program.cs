using Identity.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Easy.Services.Sso
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();
            builder.Run();
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
