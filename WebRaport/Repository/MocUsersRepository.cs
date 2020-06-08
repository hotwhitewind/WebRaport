using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Interfaces;
using WebRaport.Models;

namespace WebRaport.Repository
{
    public class MocUsersRepository : IUsersRepository
    {
        private readonly IConfiguration _config;

        public MocUsersRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<List<User>> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
