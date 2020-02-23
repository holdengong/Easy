using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Sso.Assembly
{
    public static class EasySsoConsts
    {
        public const string CookieSchema = "EasySsoCookies";
        public const string OidcSchema = "EasySsoOidc";
        public const string CookieName = "Easy.Identity.Cookies";
        public const string DefaultLoginPath = "/Account/Login";
        public const string Authority = "https://localhost:10001";
        public static readonly List<string> DefaultScope = new List<string>
        {
            "api",
            "openid",
            "profile",
            "userid",
            "username",
            "email",
            "mobile",
            "offline_access"
        };

        public const string TokenEndpoint = "/extension/gettoken";

        public const string UserInfoEndpoint = "/extension/getuserinfo";
    }
}
