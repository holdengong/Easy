using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Sso.Assembly
{
    public class EasyIdentityDbContext : IdentityDbContext
    {
        public EasyIdentityDbContext(DbContextOptions<EasyIdentityDbContext> dbContextOptions)
            :base(dbContextOptions)
        {
        }
    }
}
