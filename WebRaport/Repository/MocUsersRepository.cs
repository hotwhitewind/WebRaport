using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Interfaces;
using WebRaport.Models;
using Dapper;
using System.Data;
using Microsoft.Extensions.Logging;

namespace WebRaport.Repository
{
    public class MocUsersRepository : IUsersRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MocUsersRepository> _logger;

        public MocUsersRepository(IConfiguration configuration, ILogger<MocUsersRepository> logger)
        {
            _config = configuration;
            _logger = logger;
        }

        public async Task<User> GetUserById(int Id)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<User>("SELECT * FROM Users WHERE UserId = @Id", new { Id });

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task<string> GetUserFiledValueByColumnName(int Id, string ColumnName)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<string>($"SELECT {ColumnName} FROM Users WHERE UserId = @Id", new { Id });

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task<List<User>> GetUsers()
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<User>("SELECT * FROM Users");

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }
    }
}
