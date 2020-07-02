using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Models;

namespace WebRaport.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> Create(User user);
        Task Delete(int id);
        Task<User> Get(int id);
        Task<List<int>> GetUserIdByLoginName(string LoginName);
        Task<string> GetLoginByUserId(int UserId);
        Task<List<User>> GetUsers();
        Task Update(User user);
        Task<string> GetUserFieldValueByColumnName(int Id, string ColumnName);
        Task UpdatePassword(int UserID, string newPassword);
        Task<User> IsAuthentificate(string Login, string Password);
    }
}
