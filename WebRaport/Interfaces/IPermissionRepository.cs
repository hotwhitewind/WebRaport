using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Models;

namespace WebRaport.Interfaces
{
    public interface IPermissionRepository
    {
        Task Create(Permission permission);
        void Delete(int id);
        Task<Permission> Get(int id);
        Task<Permission> Get(string Name);

        Task<Permission> GetByUserId(int UserID);
        Task<List<Permission>> GetPermissions();
        Task<List<string>> GetPermissionNames();
        void Update(Permission user);
        Task AddPermissionForUserByName(int UserID, string permissionName);
        Task AddPermissionForUserByID(int UserID, int permissionID);
        Task ChangePermissionForUserByName(int UserID, string permissionName);

    }
}
