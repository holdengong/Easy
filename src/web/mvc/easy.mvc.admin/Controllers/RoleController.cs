using Easy.Mvc.Admin.DbContexts;
using Easy.Mvc.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Mvc.Admin.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(AdminDbContext adminDbContext, RoleManager<IdentityRole> roleManager)
        {
            _adminDbContext = adminDbContext;
            _roleManager = roleManager;
        }

        private readonly AdminDbContext _adminDbContext;

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Route("api/roles")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]RoleDto dto)
        {
            var entity = new IdentityRole
            {
                Name = dto.Name
            };

            await _roleManager.CreateAsync(entity);

            return EasyResult.Ok();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/role/{id}")]
        public async Task<IActionResult> Update([FromRoute]string id, [FromBody]RoleDto dto)
        {
            var entity = await _roleManager.FindByIdAsync(id);
            if (entity != null)
            {
                entity.Name = dto.Name;
                await _roleManager.UpdateAsync(entity);
            }

            return EasyResult.Ok();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/role/{id}")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);
            if (entity != null)
            {
                await _roleManager.DeleteAsync(entity);
            }

            return EasyResult.Ok();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/role/{id}")]
        public async Task<IActionResult> Get([FromRoute]string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if (entity != null)
            {
                var result = new RoleDto
                {
                    Id = id,
                    Name = entity.Name
                };

                return EasyResult.Ok(result);
            }

            return EasyResult.Ok();
        }

        [Route("api/roles")]
        [HttpGet]
        public IActionResult List([FromQuery]int pageIndex, int pageSize, string keywords)
        {
            var roles = _roleManager.Roles;

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                roles = roles.Where(_ => _.Name.Contains(keywords));
            }

            int total = roles.Count();

            int skip = (pageIndex - 1) * pageSize;
            roles = roles.Skip(skip).Take(pageSize);

            return EasyResult.PagedList(roles, total);
        }

    }
}
