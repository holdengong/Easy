using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Mvc.Admin.Models
{
    public class PermissionDto
    {
        public string Id { get; set; }
        public PermissionType Type { get; set; }
        public string Path { get; set; }
        public string ParentId { get; set; }
        public string Code { get; set; }
        public string HierarchyCode { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
    }
}
