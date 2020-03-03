using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Mvc.Admin.Models
{
    public class BusinessUnitDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public BusinessUnitType Type { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 关联id, 如Type为角色则为RoleId, 如Type为用户则为UserId
        /// </summary>
        public string RefId { get; set; }
    }
}
