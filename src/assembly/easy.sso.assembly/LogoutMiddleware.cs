using Easy.Sso.Assembly;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasyAuth
{
    public class LogoutMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 管道执行到该中间件时候下一个中间件的RequestDelegate请求委托，如果有其它参数，也同样通过注入的方式获得
        /// </summary>
        /// <param name="next"></param>
        public LogoutMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            //通过注入方式获得对象
            _next = next;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.ToLower() == EasySsoConsts.LOGOUT_ENDPOINT)
            {
                await context.SignOutAsync(EasySsoConsts.CookieSchema);
                await context.SignOutAsync(EasySsoConsts.OidcSchema,new AuthenticationProperties 
                {
                     RedirectUri = context.Request.Headers["Referer"]
                });
            }

            await _next.Invoke(context);
        }
    }
}
