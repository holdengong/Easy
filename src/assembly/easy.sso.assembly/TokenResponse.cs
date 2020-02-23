using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAuth
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public DateTime ExpiressAt { get; set; }
    }
}
