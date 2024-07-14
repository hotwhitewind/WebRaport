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
        Task<List<RaportModel>> GetCreatedRaports();
        Task<RaportModel> GetCreatingRaportByUserId(int UserId);
        Task<List<FieldModel>> GetFieldsByRaportId(int Id);
        Task<string> GetHandlerPageName(int RaportId);
        Task<bool> CreateRaport(RaportModel raport);
        Task SetRaportCreated(int RaportId);
        Task UpdateRaport(RaportModel raport);
        Task UpdateRaportTemplateFilePath(int RaportId, string path, int editUserId);
        Task<bool> AddFieldIntoRaport(int RaportId, int FieldId);
        Task DeleteRaport(int Id);
        Task<RaportModel> GetRaportById(int Id);
    }
}
