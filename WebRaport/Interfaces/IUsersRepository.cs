using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Models;

namespace WebRaport.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<User>> GetUsers();
    }
}
