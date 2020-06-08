using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebRaport.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Rank { get; set; }
    }
}
