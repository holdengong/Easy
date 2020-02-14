using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebStandard.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
