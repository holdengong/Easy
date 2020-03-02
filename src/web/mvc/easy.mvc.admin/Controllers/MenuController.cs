using Easy.Mvc.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Easy.Mvc.Admin.Controllers
{
    public class MenuController : Controller
    {
        //[Authorize]
        [HttpGet("api/menus")]
        public IActionResult List()
        {
            var result = new List<MenuViewModel>();

            result.Add(new MenuViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "用户管理",
                Children = new List<MenuViewModel>
                {
                  new MenuViewModel("用户列表","/users"),
                }

            });

            result.Add(new MenuViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "权限管理",
                Children = new List<MenuViewModel>
                {
                  new MenuViewModel("角色列表","/roles"),
                  new MenuViewModel("权限列表","/permissions")
                }
            });

            result.Add(new MenuViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "商品管理",
                Children = new List<MenuViewModel>
                {
                  new MenuViewModel("商品列表","/commodity/list"),
                  new MenuViewModel("分类参数","/commodity/args"),
                  new MenuViewModel("商品分类","/commodity/category")
                }
            });

            result.Add(new MenuViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "订单管理",
                Children = new List<MenuViewModel>
                {
                  new MenuViewModel("订单列表","/order/list"),
                }
            });

            result.Add(new MenuViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "数据统计",
                Children = new List<MenuViewModel>
                {
                  new MenuViewModel("统计报表","/statistics/list"),
                }
            });

            return EasyResult.Ok(result);
        }
    }
}
