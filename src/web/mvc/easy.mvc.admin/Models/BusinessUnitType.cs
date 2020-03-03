using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Mvc.Admin.Models
{
    /// <summary>
    /// 商业单位类型
    /// </summary>
    public enum BusinessUnitType
    {
        /// <summary>
        /// 集团、公司、部门、小组等组织结构
        /// </summary>
        Department,

        /// <summary>
        /// 角色
        /// </summary>
        Role,

        /// <summary>
        /// 员工
        /// </summary>
        Employee
    }
}
