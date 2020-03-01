using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Common.Assembly
{
    public class PagedReturnBody<T>
        where T:class,new ()
    {
        public IEnumerable<T> List { get; set; }
        public int Total { get; set; }
    }
}
