using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.SeedWorks
{
    public static class SeedWork
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            ConfigurationDatabase.Initialize(serviceProvider);
            GrantsDatabase.Initialize(serviceProvider);
            IdentityDatabase.Initialize(serviceProvider);
        }
    }
}
