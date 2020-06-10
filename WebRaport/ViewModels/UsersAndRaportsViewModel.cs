using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebRaport.ViewModels
{
    public class UsersAndRaportsViewModel
    {
        public int UserID { get; set; }
        public int RaportId { get; set; }
        public List<SelectListItem> UsersList { get; set; }
        public List<SelectListItem> RaportsList { get; set; }

    }
}
