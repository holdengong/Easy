using Easy.Common.Assembly;
using Easy.Mvc.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Mvc.Admin.DbContexts
{
    /// <summary>
    /// 商业单位
    /// </summary>
    public class BusinessUnit : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(255)]
        public string Name { get; set; }
        
        /// <summary>
        /// 类型
        /// </summary>
        public BusinessUnitType Type { get; set; }
        

        /// <summary>
        /// 父级id
        /// </summary>
        [MaxLength(50)]
        public string ParentId { get; set; }
    
        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(10)]
        public string Code { get; set; }

        /// <summary>
        /// 关联id, 如Type为角色则为RoleId, 如Type为用户则为UserId
        /// </summary>
        public string RefId { get; set; }
    }
}
