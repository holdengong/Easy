using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyAuth
{
    public class EasyIdentityDbContext : IdentityDbContext
    {
        public EasyIdentityDbContext(DbContextOptions<EasyIdentityDbContext> options)
            :base(options)
        { 
        }
    }
}
