using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Identity.API.SeedWorks
{
    public static class GrantsDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<PersistedGrantDbContext>())
            {
            }
        }
    }
}
