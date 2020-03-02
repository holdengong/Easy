using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Mvc.Admin.Models
{
    /// <summary>
    /// 权限树
    /// </summary>
    public class PermissionDtoForTree
    {
        public string Id { get; set; }
        public PermissionType Type { get; set; }
        public string Path { get; set; }
        public string Code { get; set; }
        public string HierarchyCode { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public List<PermissionDtoForTree> Children { get; set; }
    }
}
