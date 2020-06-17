using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Interfaces;
using WebRaport.Models;

namespace WebRaport.Repository
{
    public class MocPermissionRepository : IPermissionRepository
    {
        private readonly IConfiguration _config;

        public MocPermissionRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task Create(Permission permission)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "INSERT INTO Permissions (PermissionId, Name, Description) VALUES (@PermissionId, @Name, @Description);";
                    await db.ExecuteAsync(sqlQuery, permission);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Permission> Get(int id)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result =
                        await db.QueryFirstOrDefaultAsync<Permission>(
                            "SELECT * FROM Permissions WHERE PermissionId = @id", new { id });
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<Permission> Get(string Name)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryFirstOrDefaultAsync<Permission>("SELECT * FROM Permissions WHERE Name = @Name", new { Name });
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }

        public async Task<Permission> GetByUserId(int UserId)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var userPermission = await db.QueryFirstOrDefaultAsync<Permission>(
                        "SELECT * FROM UserPermissions WHERE UserId = @UserId", new { UserId });

                    if (userPermission == null) return null;
                    var permission = await Get(userPermission.PermissionId);
                    return permission;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<List<Permission>> GetPermissions()
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<Permission>("SELECT * FROM Permissions");
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<List<string>> GetPermissionNames()
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<Permission>("SELECT * FROM Permissions");
                    return result.Select((c) => c.Name).ToList();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public void Update(Permission user)
        {
            throw new NotImplementedException();
        }

        public async Task AddPermissionForUserByName(int UserId, string permissionName)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await Get(permissionName);
                    var sqlQuery = "INSERT INTO UserPermissions (UserId, PermissionId) VALUES (@UserId, @PermissionId)";
                    await db.ExecuteAsync(sqlQuery, new { UserId, PermissionId = result.PermissionId });
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task ChangePermissionForUserByName(int UserId, string permissionName)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await Get(permissionName);
                    var sqlQuery = "UPDATE UserPermissions SET PermissionId = @PermissionID WHERE UserId = @UserId";
                    await db.ExecuteAsync(sqlQuery, new { UserId, PermissionId = result.PermissionId });
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task AddPermissionForUserByID(int UserId, int permissionId)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery = "INSERT INTO UserPermissions (UserId, PermissionId) VALUES (@UserId, @PermissionId)";
                    await db.ExecuteAsync(sqlQuery, new { UserId, PermissionId = permissionId });
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
