using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Admin.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [Authorize]
        public IActionResult Index()
        {
            var viewModel = new ClaimViewModel
            {
                Claims = User.Claims.ToList()
            };
            return View(viewModel);
        }
    }

    public class ClaimViewModel
    { 
        public List<Claim> Claims { get; set; }
    }
}
