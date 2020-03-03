﻿using Easy.Mvc.Admin.DbContexts;
using Easy.Mvc.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Mvc.Admin.Controllers
{
    public class PermissionController : Controller
    {
        public PermissionController(AdminDbContext adminDbContext)
        {
            AdminDbContext = adminDbContext;
        }

        public AdminDbContext AdminDbContext { get; }

        /// <summary>
        /// 新增权限
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Route("api/permissions")]
        [HttpPost]
        public IActionResult AddPermission([FromBody]PermissionDto viewModel)
        {
            var sameCodePermission = AdminDbContext.Permissions
                .FirstOrDefault(_ => _.ParentId == viewModel.ParentId && _.Code == viewModel.Code);
            if (sameCodePermission != null)
            {
                return EasyResult.Error("编码已存在");
            }

            var sort = AdminDbContext.Permissions.Count(_ => _.ParentId == viewModel.ParentId) + 1;

            var entity = new Permission
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
            }.OpsBeforeAdd<Permission>(HttpContext);

            AdminDbContext.Permissions.Add(entity);
            AdminDbContext.SaveChanges();

            return EasyResult.Ok();
        }

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/permission/{id}")]
        public IActionResult UpdatePermission([FromRoute]string id,[FromBody]PermissionDto viewModel)
        {
            var entity = AdminDbContext.Permissions.FirstOrDefault(_ => _.Id == id);
            if (entity != null)
            {
                entity.Code = viewModel.Code;
                entity.Name = viewModel.Name;
                entity.Path = viewModel.Path;
                entity.Type = viewModel.Type;
                entity.Remarks = viewModel.Remarks;
                entity.Icon = viewModel.Icon;
                AdminDbContext.Permissions.Update(entity);
                AdminDbContext.SaveChanges();
            }

            return EasyResult.Ok();
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/permission/{id}")]
        public IActionResult DeletePermission([FromRoute]string id)
        {
            var entity = AdminDbContext.Permissions.FirstOrDefault(_ => _.Id == id);
            if (entity != null)
            {
                var hasChildren = AdminDbContext.Permissions.Count(_ => _.ParentId == id) > 0;

                if (hasChildren)
                {
                    return EasyResult.Error("存在子级，不允许删除");
                }

                AdminDbContext.Permissions.Remove(entity);
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
        [Route("api/permission/{id}")]
        public IActionResult GetById([FromRoute]string id)
        {
            var entity = AdminDbContext.Permissions.FirstOrDefault(_ => _.Id == id);
            if (entity != null)
            {
                var result = new PermissionDto
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
        [Route("api/permission/{id}/position")]
        public IActionResult MovePosition([FromRoute]string id,[FromQuery]string action)
        {
            var entity = AdminDbContext.Permissions.FirstOrDefault(_ => _.Id == id);
            if (entity != null)
            {
                if (action == "up")
                {
                    var previousEntity = AdminDbContext.Permissions.FirstOrDefault(_ => _.ParentId == entity.ParentId && _.Sort == entity.Sort - 1);
                    if (previousEntity != null)
                    {
                        previousEntity.Sort += 1;
                        entity.Sort -= 1;
                    }
                    AdminDbContext.SaveChanges();
                }

                if (action == "down")
                {
                    var nextEntity = AdminDbContext.Permissions.FirstOrDefault(_ => _.ParentId == entity.ParentId && _.Sort == entity.Sort + 1);
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
        /// 获取权限树
        /// </summary>
        /// <param name="scope">范围 menu：只返回菜单</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/permissions")]
        public IActionResult GetAll([FromQuery]string scope)
        {
            var entities = AdminDbContext.Permissions.ToList();

            if (scope == "menu")
            {
                entities = entities.Where(_ => _.Type == PermissionType.菜单).ToList();
            }

            var result = ConvertEntitiesToTree(entities);

            return EasyResult.Ok(result);
        }

        private List<PermissionDtoForTree> ConvertEntitiesToTree(List<Permission> allPermissions)
        {
            var result = new List<PermissionDtoForTree>();

            var topPermissions = allPermissions.Where(_ => string.IsNullOrWhiteSpace(_.ParentId))
                .OrderBy(_=>_.Sort).ThenByDescending(_=>_.CreatedOn);

            foreach (var topPermission in topPermissions)
            {
                result.Add(RecursivelyConvertToTree(topPermission, allPermissions));
            }

            return result;
        }

        private PermissionDtoForTree RecursivelyConvertToTree(Permission entity, List<Permission> entities, PermissionDtoForTree branch = null)
        {
            if (branch == null)
            {
                branch = new PermissionDtoForTree
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
                branch.Children = new List<PermissionDtoForTree>();
                foreach (var childEntity in childrenEntities)
                {
                    var branchChild = new PermissionDtoForTree
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