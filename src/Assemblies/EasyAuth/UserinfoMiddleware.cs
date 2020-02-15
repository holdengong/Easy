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
    public class UserInfoMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 管道执行到该中间件时候下一个中间件的RequestDelegate请求委托，如果有其它参数，也同样通过注入的方式获得
        /// </summary>
        /// <param name="next"></param>
        public UserInfoMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            //通过注入方式获得对象
            _next = next;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.ToLower() == EasyAuthConsts.UserInfoEndpoint)
            {
                await GetUserInfoAsync(context);
                return;
            }

            await _next.Invoke(context);
        }

        private async Task GetUserInfoAsync(HttpContext context)
        {
            string clientId = Configuration["ClientId"];
            string clientSecret = Configuration["ClientSecret"];
            string ssoHost = Configuration["SsoHost"];

            var accessToken = await context.GetTokenAsync("access_token");
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();

            var serverClient = httpClientFactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync(ssoHost);

            var userInfoClient = httpClientFactory.CreateClient();

            var userInfoResponse = await userInfoClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = discoveryDocument.UserInfoEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Token = accessToken,
            });

            if (userInfoResponse.IsError)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            var response = new UserInfoResponse
            {
                UserId = userInfoResponse.Claims.FirstOrDefault(_ => _.Type == "userid")?.Value,
                Email = userInfoResponse.Claims.FirstOrDefault(_ => _.Type == "email")?.Value,
                Mobile = userInfoResponse.Claims.FirstOrDefault(_ => _.Type == "mobile")?.Value,
                UserName = userInfoResponse.Claims.FirstOrDefault(_ => _.Type == "username")?.Value
            };

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "application/json; charset=UTF-8";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response), Encoding.UTF8);
            return;
        }
    }
}
