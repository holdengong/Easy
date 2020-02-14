using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OcelotApiGw.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [Route("/test")]
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("result");
        }
    }
}
