using Easy.Sso.Assembly;
using IdentityModel.Client;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasyAuth
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 管道执行到该中间件时候下一个中间件的RequestDelegate请求委托，如果有其它参数，也同样通过注入的方式获得
        /// </summary>
        /// <param name="next"></param>
        public TokenMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            //通过注入方式获得对象
            _next = next;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.ToLower() == EasySsoConsts.TOKEN_ENDPOINT)
            {
                await GetTokenAsync(context);
                return;
            }

            await _next.Invoke(context);
        }

        private async Task GetTokenAsync(HttpContext context)
        {
            string clientId = Configuration["ClientId"];
            string clientSecret = Configuration["ClientSecret"];
            string ssoHost = Configuration["SsoHost"];

            var accessToken = await context.GetTokenAsync("access_token");
            var tokenType = await context.GetTokenAsync("token_type");
            var expiresAt = await context.GetTokenAsync("expires_at");
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(tokenType) || string.IsNullOrEmpty(expiresAt))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            var response = new TokenResponse
            {
                AccessToken = accessToken,
                TokenType = tokenType,
                ExpiressAt = DateTime.Parse(expiresAt)
            };

            if (DateTime.Now >= response.ExpiressAt)
            {
                response = await RefreshTokenAsync(context, clientId, clientSecret, ssoHost);
            }

            if (response == null)
            {
                return;
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "application/json; charset=UTF-8";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response), Encoding.UTF8);
            return;
        }

        private async Task<TokenResponse> RefreshTokenAsync(HttpContext context,string clientId,string clientSecret,string ssoHost)
        {
            var refreshToken = await context.GetTokenAsync("refresh_token");
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return null;
            }

            var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();

            var serverClient = httpClientFactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync(ssoHost);

            var refreshTokenClient = httpClientFactory.CreateClient();

            var refreshTokenHash = (refreshToken + ";" + "refresh_token").Sha256();

            var tokenResponse = await refreshTokenClient.RequestRefreshTokenAsync(
                new RefreshTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    RefreshToken = refreshToken,
                    ClientId = clientId,
                    ClientSecret = clientSecret
                });

            if (tokenResponse.IsError)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return null; 
            }

            var authInfo = await context.AuthenticateAsync(EasySsoConsts.CookieSchema);
            authInfo.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
            authInfo.Properties.UpdateTokenValue("id_token", tokenResponse.IdentityToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);

            await context.SignInAsync(EasySsoConsts.CookieSchema, authInfo.Principal, authInfo.Properties);

            return new TokenResponse
            {
                AccessToken = tokenResponse.AccessToken,
                ExpiressAt = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn),
                TokenType = tokenResponse.TokenType
            };
        }
    }
}
