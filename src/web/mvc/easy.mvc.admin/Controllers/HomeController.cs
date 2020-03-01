using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Easy.Mvc.Admin.Controllers
{
    //[Authorize]
    [Route("{*url}", Order = 999)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
