using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Mvc.Admin.Models
{
    public class MenuViewModel
    {
        public MenuViewModel()
        { }

        public MenuViewModel(string name,string path)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Path = path;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public List<MenuViewModel> Children { get; set; }
    }
}
