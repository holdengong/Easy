using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Easy.Mvc.Sso.Data
{
    public class EasyIdentityDbContext : IdentityDbContext
    {
        public EasyIdentityDbContext(DbContextOptions<EasyIdentityDbContext> options)
            : base(options)
        {
        }
    }
}
