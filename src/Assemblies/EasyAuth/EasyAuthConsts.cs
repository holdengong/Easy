using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAuth
{
    public static class EasyAuthConsts
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
            "userid",
            "username",
            "email",
            "mobile",
            "offline_access"
        };
    }
}
