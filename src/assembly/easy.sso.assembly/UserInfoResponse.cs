using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAuth
{
    public class UserInfoResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}
