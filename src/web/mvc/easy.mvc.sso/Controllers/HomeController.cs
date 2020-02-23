using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("{*url}", Order = 999)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
