using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebRaport.Authorization
{
    public class AuthRequired : IAuthorizationRequirement
    {
        public List<string> RoleName { get; set; }
        public AuthRequired(string[] rolesName)
        {
            RoleName = rolesName.ToList();
        }
    }
}
