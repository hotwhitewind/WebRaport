using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Models;

namespace WebRaport.Interfaces
{
    public interface IRaportRepository
    {
        Task<List<RaportModel>> GetRaports();
        Task<List<FieldModel>> GetFieldsByRaportId(int Id);
        Task<bool> CreateRaport(RaportModel raport);
        Task DeleteRaport(int Id);
        Task<RaportModel> GetRaportById(int Id);
    }
}
