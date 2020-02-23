using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data
{
    public class EasyIdentityDbContext : IdentityDbContext
    {
        public EasyIdentityDbContext(DbContextOptions<EasyIdentityDbContext> options)
            : base(options)
        {
        }
    }
}
