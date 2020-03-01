using Easy.Mvc.Sso.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Identity.API.SeedWorks
{
    public static class IdentityDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<EasyIdentityDbContext>())
            {
            }
        }
    }
}
