using Easy.Common.Assembly;
using Easy.Mvc.Sso.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [Route("api/account")]
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
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel vm)
        {
            var user = await _userManager.FindByNameAsync(vm.Username);
            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false);
                if (signInResult.Succeeded)
                {
                    return EasyResponse.Success();
                }

                return EasyResponse.Fail("用户名或密码错误");
            }

            return EasyResponse.Fail("用户名或密码错误");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(string username, string password, string email, string mobile)
        {
            var user = new IdentityUser(username);
            await _userManager.CreateAsync(user, password);
            var claims = new List<Claim>
            {
                new Claim("userid",user.Id),
                new Claim("username",user.UserName),
                new Claim("email",email),
                new Claim("mobile",mobile)
            };
            await _userManager.AddClaimsAsync(user, claims);
            return Ok();
        }
    }
}
