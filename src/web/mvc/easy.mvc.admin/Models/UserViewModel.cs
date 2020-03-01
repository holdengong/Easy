using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Mvc.Admin.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
    }
}
