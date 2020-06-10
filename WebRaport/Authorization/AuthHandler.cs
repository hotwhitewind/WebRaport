using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebRaport.Authorization
{
    public class AuthHandler : AuthorizationHandler<AuthRequired>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthRequired requirement)
        {
            var user = context.User;
            var claim = context.User.FindFirst("AdminRequiredPermission");
            if (claim != null)
            {
                var authName = claim.Value;
                if (authName == requirement.RoleName)
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
