using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyShopClient.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IHttpClientFactory httpClientFactory)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl)
        {
            var httpClient = _httpClientFactory.CreateClient();
            returnUrl = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded)

                {
                    var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:10001");
                    var token = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = "easyshop_client",
                        ClientSecret = "secret",
                        UserName = username,
                        Password = password
                    });

                    Response.Cookies.Append("access_token", token.AccessToken);

                    return Redirect(returnUrl);
                }
            }

            return RedirectToAction(returnUrl);
        }

        public IActionResult Login([FromQuery]string returnUrl)
        {
            returnUrl = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;
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
