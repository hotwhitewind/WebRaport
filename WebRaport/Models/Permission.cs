using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebRaport.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UserPermissions
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
    }
}
