using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var identityApiConfig = Configuration.GetSection("IdentityApiConfig").Get<IdentityApiConfig>();

            bool isQuickStart = Configuration.GetValue<bool>("IsQuickStart");

            if (isQuickStart)
            {
                var identityServerBuilder = services.AddEasyIdentityServerInMemory(identityApiConfig);
                services.AddEasyIdentityInMemory(identityApiConfig, identityServerBuilder);
            }
            else
            {
                var identityServerBuilder = services.AddEasyIdentityServer(identityApiConfig);
                services.AddEasyIdentityMySql(identityApiConfig, identityServerBuilder);
            }

            services.AddEasySso("identity");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
