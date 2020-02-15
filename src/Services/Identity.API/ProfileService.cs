using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.API
{
    public class ProfileService : DefaultProfileService
    {
        public ProfileService(ILogger<DefaultProfileService> logger) 
            : base(logger)
        {
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(Logger);

            if (context.RequestedClaimTypes.Any())
            {
                context.IssuedClaims.AddRange(FilterClaims(context, context.Subject.Claims));
            }

            context.LogIssuedClaims(Logger);

            return Task.CompletedTask;
        }

        public List<Claim> FilterClaims(ProfileDataRequestContext context, IEnumerable<Claim> claims)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (claims == null)
            {
                throw new ArgumentNullException("claims");
            }
            return (from x in claims
                    where context.RequestedClaimTypes.Contains(x.Type)
                    select x).ToList();
        }
    }
}
