using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password,string returnUrl)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded)
                {
                    return Redirect(returnUrl);
                }
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login([FromQuery]string returnUrl)
        {
            returnUrl = string.IsNullOrWhiteSpace(returnUrl) ? "https://localhost:20000" : returnUrl;
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser(username);
            await _userManager.CreateAsync(user, password);
            return Ok();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
