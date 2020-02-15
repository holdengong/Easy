using EasyAuth;

namespace Microsoft.AspNetCore.Builder
{
    public static class EasyAuthMiddlewareExtensisons
    {
        public static IApplicationBuilder UseEasyAuth(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<TokenMiddleware>()
                .UseMiddleware<UserInfoMiddleware>();
            return builder;
        }
    }
}
