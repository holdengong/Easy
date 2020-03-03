using Easy.Mvc.Admin.DbContexts;
using Easy.Mvc.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Mvc.Admin.Controllers
{
    public class MenuController : Controller
    {
        public MenuController(AdminDbContext adminDbContext)
        {
            AdminDbContext = adminDbContext;
        }

        public AdminDbContext AdminDbContext { get; }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Route("api/menu")]
        [HttpPost]
        public IActionResult Add([FromBody]MenuDto viewModel)
        {
            var sameCodePermission = AdminDbContext.Menus
                .FirstOrDefault(_ => _.ParentId == viewModel.ParentId && _.Code == viewModel.Code);
            if (sameCodePermission != null)
            {
                return EasyResult.Error("编码已存在");
            }

            var sort = AdminDbContext.Menus.Count(_ => _.ParentId == viewModel.ParentId) + 1;

            var entity = new Menu
            {
                Code = viewModel.Code,
                ParentId = viewModel.ParentId,
                Name = viewModel.Name,
                Path = viewModel.Path,
                Type = viewModel.Type,
                Remarks = viewModel.Remarks,
                Id = Guid.NewGuid().ToString(),
                Sort = sort,
                Icon = viewModel.Icon
            }.OpsBeforeAdd<Menu>(HttpContext);

            AdminDbContext.Menus.Add(entity);
            AdminDbContext.SaveChanges();

            return EasyResult.Ok();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/menu/{id}")]
        public IActionResult Update([FromRoute]string id,[FromBody]MenuDto viewModel)
        {
            var entity = AdminDbContext.Menus.FirstOrDefault(_ => _.Id == id);
            if (entity != null)
            {
                entity.Code = viewModel.Code;
                entity.Name = viewModel.Name;
                entity.Path = viewModel.Path;
                entity.Type = viewModel.Type;
                entity.Remarks = viewModel.Remarks;
                entity.Icon = viewModel.Icon;
                AdminDbContext.Menus.Update(entity);
                AdminDbContext.SaveChanges();
            }

            return EasyResult.Ok();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/menu/{id}")]
        public IActionResult Delete([FromRoute]string id)
        {
            var entity = AdminDbContext.Menus.FirstOrDefault(_ => _.Id == id);
            if (entity != null)
            {
                var hasChildren = AdminDbContext.Menus.Count(_ => _.ParentId == id) > 0;

                if (hasChildren)
                {
                    return EasyResult.Error("存在子级，不允许删除");
                }

                AdminDbContext.Menus.Remove(entity);
                AdminDbContext.SaveChanges();
            }

            return EasyResult.Ok();
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/menu/{id}")]
        public IActionResult GetById([FromRoute]string id)
        {
            var entity = AdminDbContext.Menus.FirstOrDefault(_ => _.Id == id);
            if (entity != null)
            {
                var result = new MenuDto
                {
                    Id = entity.Id,
                    ParentId = entity.ParentId,
                    Code = entity.Code,
                    Name = entity.Name,
                    Path = entity.Path,
                    Remarks = entity.Remarks,
                    Type = entity.Type,
                    Icon = entity.Icon
                };

                return EasyResult.Ok(result);
            }

            return EasyResult.Ok();
        }

        [HttpPut]
        [Route("api/menu/{id}/position")]
        public IActionResult MovePosition([FromRoute]string id,[FromQuery]string action)
        {
            var entity = AdminDbContext.Menus.FirstOrDefault(_ => _.Id == id);
            if (entity != null)
            {
                if (action == "up")
                {
                    var previousEntity = AdminDbContext.Menus.FirstOrDefault(_ => _.ParentId == entity.ParentId && _.Sort == entity.Sort - 1);
                    if (previousEntity != null)
                    {
                        previousEntity.Sort += 1;
                        entity.Sort -= 1;
                    }
                    AdminDbContext.SaveChanges();
                }

                if (action == "down")
                {
                    var nextEntity = AdminDbContext.Menus.FirstOrDefault(_ => _.ParentId == entity.ParentId && _.Sort == entity.Sort + 1);
                    if (nextEntity != null)
                    {
                        nextEntity.Sort -= 1;
                        entity.Sort += 1;
                    }
                    AdminDbContext.SaveChanges();
                }

                return EasyResult.Ok();
            }

            return EasyResult.Ok();
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <param name="scope">范围 menu：只返回菜单</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/menus")]
        public IActionResult GetAll([FromQuery]string scope)
        {
            var entities = AdminDbContext.Menus.ToList();

            if (scope == "menu")
            {
                entities = entities.Where(_ => _.Type == PermissionType.菜单).ToList();
            }

            var result = ConvertEntitiesToTree(entities);

            return EasyResult.Ok(result);
        }

        private List<MenuDtoForTree> ConvertEntitiesToTree(List<Menu> allmenu)
        {
            var result = new List<MenuDtoForTree>();

            var topmenu = allmenu.Where(_ => string.IsNullOrWhiteSpace(_.ParentId))
                .OrderBy(_=>_.Sort).ThenByDescending(_=>_.CreatedOn);

            foreach (var topPermission in topmenu)
            {
                result.Add(RecursivelyConvertToTree(topPermission, allmenu));
            }

            return result;
        }

        private MenuDtoForTree RecursivelyConvertToTree(Menu entity, List<Menu> entities, MenuDtoForTree branch = null)
        {
            if (branch == null)
            {
                branch = new MenuDtoForTree
                {
                    Code = entity.Code,
                    HierarchyCode = entity.Code,
                    Id = entity.Id,
                    Name = entity.Name,
                    Path = entity.Path,
                    Remarks = entity.Remarks,
                    Type = entity.Type,
                    Icon = string.IsNullOrWhiteSpace(entity.Icon) ? "el-icon-menu"
                        : entity.Icon
                };
            }

            var childrenEntities = entities.Where(_ => _.ParentId == entity.Id)
                .OrderBy(_=>_.Sort).ThenByDescending(_=>_.CreatedOn);

            if (childrenEntities.Any())
            {
                branch.Children = new List<MenuDtoForTree>();
                foreach (var childEntity in childrenEntities)
                {
                    var branchChild = new MenuDtoForTree
                    {
                        Id = childEntity.Id,
                        Code = childEntity.Code,
                        HierarchyCode = $"{branch.HierarchyCode}.{childEntity.Code}",
                        Name = childEntity.Name,
                        Path = childEntity.Path,
                        Type = childEntity.Type,
                        Remarks = childEntity.Remarks,
                        Icon = string.IsNullOrWhiteSpace(childEntity.Icon) ? "el-icon-menu"
                        : childEntity.Icon
                    };

                    branchChild = RecursivelyConvertToTree(childEntity, entities, branchChild);

                    branch.Children.Add(branchChild);
                }
            }

            return branch;
        }
    }
}
