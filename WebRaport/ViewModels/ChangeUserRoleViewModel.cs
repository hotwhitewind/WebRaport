using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Models;

namespace WebRaport.ViewModels
{
    public class ChangeUserRoleViewModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CurrentRole { get; set; }
        public List<Permission> selectRoles { get; set; }
    }
}
