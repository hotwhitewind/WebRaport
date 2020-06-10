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
using System.Security.Cryptography;
using System.Text;

namespace WebRaport.Repository
{
    public class MocUsersRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MocUsersRepository> _logger;
        private readonly IPermissionRepository _permissions;


        public MocUsersRepository(IConfiguration configuration, IPermissionRepository permission, ILogger<MocUsersRepository> logger)
        {
            _config = configuration;
            _logger = logger;
            _permissions = permission;
        }

        public async Task<bool> Create(User user)
        {
            if (user.Role == null || (user.Role.PermissionID == 0 && string.IsNullOrEmpty(user.Role.Name)))
                return false;
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    user.Password = ComputeHash(user.Password);
                    var sqlQuery =
                        "INSERT INTO Users (Login,FirstName,LastName,SecondName,Password,Position,Rank,PersonalNumber,BirthDay) " +
                        "VALUES (@Login, @FirstName, @LastName, @SecondName,@Password,@Position,@Rank,@PersonalNumber,@BirthDay); " +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";
                    int? userIdRet = await db.QueryFirstOrDefaultAsync<int>(sqlQuery, user);
                    user.UserID = userIdRet.Value;
                    if (user.Role != null)
                    {
                        var userId = user.UserID;
                        if (user.Role.PermissionID != 0)
                        {
                            await _permissions.AddPermissionForUserByID(user.UserID, user.Role.PermissionID);
                        }
                        if (!string.IsNullOrEmpty(user.Role.Name))
                        {
                            await _permissions.AddPermissionForUserByName(user.UserID, user.Role.Name);
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return false;
                }
            }
        }

        public async Task Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "DELETE FROM Users WHERE UserID = @id;";
                    await db.ExecuteAsync(sqlQuery, new { id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public async Task<User> Get(int Id)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<User>("SELECT * FROM Users WHERE UserID = @Id", new { Id });
                    if (result.Any())
                    {
                        var permission = await _permissions.GetByUserId(result.First().UserID);
                        result.First().Role = permission;
                    }
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
                    var result = await db.QueryAsync<string>($"SELECT {ColumnName} FROM Users WHERE UserID= @Id", new { Id });

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
                    foreach (var user in result)
                    {
                        var permission = await _permissions.GetByUserId(user.UserID);
                        user.Role = permission;
                    }

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task Update(User user)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "UPDATE Users SET Login = @Login, FirstName = @FirstName," +
                        "LastName = @LastName, SecondName = @SecondName," +
                        "Password = @Password, Position = @Position, Rank = @Rank, PersonalNumber = @PersonalNumber , BirthDay = @BirthDay " +
                        "WHERE UserID = @UserID";
                    await db.ExecuteAsync(sqlQuery, user);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public async Task UpdatePassword(int UserID, string newPassword)
        {
            var Password = ComputeHash(newPassword);
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "UPDATE Users SET Password = @Password WHERE UserID = @UserID";
                    await db.ExecuteAsync(sqlQuery, new { Password, UserID });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public async Task<User> IsAuthentificate(string Login, string Password)
        {
            var passwordHash = ComputeHash(Password);
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var user =
                        await db.QueryFirstOrDefaultAsync<User>(
                            "SELECT * FROM Users WHERE Login = @Login AND Password = @passwordHash",
                            new { Login, passwordHash });
                    if (user == null) return user;
                    var permission = await _permissions.GetByUserId(user.UserID);
                    if (permission != null)
                        user.Role = permission;
                    return user;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public static string ComputeHash(string password)
        {
            SHA384CryptoServiceProvider hashAlgorithm = new SHA384CryptoServiceProvider();
            return Convert.ToBase64String(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
