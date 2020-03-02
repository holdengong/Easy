using AutoMapper;
using Easy.Mvc.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Easy.Mvc.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<IdentityUser> userManager
            ,IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [Route("api/users")]
        [HttpGet]
        public async Task<IActionResult> GetUserList([FromQuery]int pageIndex,int pageSize,string keywords)
        {
            var users = _userManager.Users;
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                users = users.Where(_=>_.UserName.Contains(keywords,StringComparison.CurrentCultureIgnoreCase));
            }

            int total = users.Count();

            int skip = (pageIndex - 1) * pageSize;
            var pagedUsers = users.Skip(skip).Take(pageSize).ToList();

            var result = new List<UserDto>();

            foreach (var user in pagedUsers)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                
                var isActive = claims.FirstOrDefault(_ => _.Type == "isactive")?.Value == null
                        ? false : bool.Parse(claims.FirstOrDefault(_ => _.Type == "isactive").Value);

                var role = string.IsNullOrWhiteSpace(claims.FirstOrDefault(_ => _.Type == "role")?.Value)
                    ? "游客" 
                    : claims.FirstOrDefault(_ => _.Type == "role")?.Value;

                result.Add(new UserDto
                {
                    Id = user.Id,
                    Mobile = user.PhoneNumber,
                    Email = user.Email,
                    IsActive = isActive,
                    UserName = user.UserName,
                    Role = role
                });
            }

            return EasyResult.PagedList(result, total);
        }

        [Route("api/users")]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody]UserDto viewModel)
        {
            var user = await _userManager.FindByNameAsync(viewModel.UserName);
            if (user != null)
            {
                return EasyResult.Error("用户已存在");
            }

            user = new IdentityUser
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                PhoneNumber = viewModel.Mobile
            };

            var addResult = await _userManager.CreateAsync(user);
            if (!addResult.Succeeded)
            {
                return EasyResult.Error("新增用户失败");
            }

            _userManager.PasswordHasher.HashPassword(user, viewModel.Password);

            return EasyResult.Ok();
        }

        [Route("api/user/{id}/state")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserStateAsync([FromRoute]string id,[FromBody]UserDto viewModel)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                var activeClaim = claims.FirstOrDefault(_ => _.Type == "isactive");
                var newActiveClaim = new Claim("isactive", viewModel.IsActive.ToString());
                if (activeClaim != null)
                {
                    var identityResult = await _userManager.ReplaceClaimAsync(user, activeClaim, newActiveClaim);
                    if (!identityResult.Succeeded)
                    {
                        return EasyResult.Error("修改用户状态失败");
                    }
                }
                else
                {
                    var identityResult = await _userManager.AddClaimAsync(user, newActiveClaim);
                    if (!identityResult.Succeeded)
                    {
                        return EasyResult.Error("修改用户状态失败");
                    }
                }
            }

            return EasyResult.Ok();
        }

        [Route("api/user/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync([FromRoute]string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return EasyResult.Error("删除用户失败");
                }
            }
            return EasyResult.Ok();
        }

        [Route("api/user/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute]string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            UserDto result = null;
            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);

                var isActive = claims.FirstOrDefault(_ => _.Type == "isactive")?.Value == null
                      ? false : bool.Parse(claims.FirstOrDefault(_ => _.Type == "isactive").Value);

                var role = string.IsNullOrWhiteSpace(claims.FirstOrDefault(_ => _.Type == "role")?.Value)
                    ? "游客"
                    : claims.FirstOrDefault(_ => _.Type == "role")?.Value;

                result = new UserDto
                {
                    Id = id,
                    Email = user.Email,
                    IsActive = isActive,
                    Mobile = user.PhoneNumber,
                    Role = role,
                    UserName = user.UserName
                };
            }
            return EasyResult.Ok(result);
        }

        [Route("api/user/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromRoute]string id,[FromBody]UserDto viewModel)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.PhoneNumber = viewModel.Mobile;
                user.Email = viewModel.Email;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return EasyResult.Error("更新用户失败");
                }
            }

            return EasyResult.Ok();
        }
    }
}
