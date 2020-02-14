using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OcelotApiGw.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [Route("/")]
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("this is secret data from gateway!");
        }
    }
}
