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
                        "INSERT INTO Permissions (PermissionID, Name, Description) VALUES (@PermissionID, @Name, @Description);";
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
                            "SELECT * FROM Permissions WHERE PermissionID = @id", new { id });
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

        public async Task<Permission> GetByUserId(int UserID)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var userPermission = await db.QueryFirstOrDefaultAsync<Permission>(
                        "SELECT * FROM UserPermissions WHERE UserID = @UserID", new { UserID });

                    if (userPermission == null) return null;
                    var permission = await Get(userPermission.PermissionID);
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

        public async Task AddPermissionForUserByName(int UserID, string permissionName)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await Get(permissionName);
                    var sqlQuery = "INSERT INTO UserPermissions (UserID, PermissionID) VALUES (@UserID, @PermissionID)";
                    await db.ExecuteAsync(sqlQuery, new { UserID, PermissionId = result.PermissionID });
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task ChangePermissionForUserByName(int UserID, string permissionName)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await Get(permissionName);
                    var sqlQuery = "UPDATE UserPermissions SET PermissionID = @PermissionID WHERE UserID = @UserID";
                    await db.ExecuteAsync(sqlQuery, new { UserID, PermissionId = result.PermissionID });
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task AddPermissionForUserByID(int UserID, int permissionID)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery = "INSERT INTO UserPermissions (UserID, PermissionID) VALUES (@UserID, @PermissionID)";
                    await db.ExecuteAsync(sqlQuery, new { UserID, PermissionId = permissionID });
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
