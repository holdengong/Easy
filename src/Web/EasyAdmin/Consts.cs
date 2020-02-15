using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.WebApp
{
    public static class Consts
    {
        public const string ClientId = "admin";
        public const string ClientSecret = "secret";
        public readonly static List<string> Scope = new List<string> { "api", "offline_access" };
        public const string LoginPath = "/User/Login";
    }
}
