using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebRaport.Authorization
{
    public class AuthRequired : IAuthorizationRequirement
    {
        public string RoleName { get; set; }
        public AuthRequired(string roleName)
        {
            RoleName = roleName;
        }
    }
}
