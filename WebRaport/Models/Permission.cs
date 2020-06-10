using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebRaport.Models
{
    public class Permission
    {
        public int PermissionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UserPermissions
    {
        public int UserID { get; set; }
        public int PermissionID { get; set; }
    }
}
