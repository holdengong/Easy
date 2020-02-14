using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data
{
    public class CustomIdentityDbContext : IdentityDbContext
    {
        public CustomIdentityDbContext(DbContextOptions options)
            :base(options)
        {
        }
    }
}
