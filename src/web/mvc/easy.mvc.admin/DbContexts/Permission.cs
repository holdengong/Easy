using Easy.Common.Assembly;
using Easy.Mvc.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Mvc.Admin.DbContexts
{
    public class Permission : BaseEntity
    {
        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        [MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        [MaxLength(50)]
        public string ParentId { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        [MaxLength(200)]
        public string Path { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string Remarks { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
