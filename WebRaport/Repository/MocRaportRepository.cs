using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Interfaces;
using WebRaport.Models;

namespace WebRaport.Repository
{
    public class MocRaportRepository : IRaportRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MocRaportRepository> _logger;


        public MocRaportRepository(IConfiguration configuration, ILogger<MocRaportRepository> logger)
        {
            _config = configuration;
            _logger = logger;
        }

        public async Task<List<RaportModel>> GetRaports()
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<RaportModel>("SELECT * FROM Raports");

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task<List<FieldModel>> GetFieldsByRaportId(int id)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<FieldModel>($"SELECT Fields.* FROM RaportFields, Fields WHERE RaportId = @id " +
                        $"AND RaportFields.FieldId = Fields.FieldId;", new {id});
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task<bool> CreateRaport(RaportModel raport)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "INSERT INTO Raports (RaportTitle, RaportData) " +
                        "VALUES (@RaportTitle, @RaportData); " +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";
                    int? raportIdRet = await db.QueryFirstOrDefaultAsync<int>(sqlQuery, raport);

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return false;
                }
            }
        }

        public async Task DeleteRaport(int Id)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "DELETE FROM Raports WHERE RaportId = @Id;";
                    await db.ExecuteAsync(sqlQuery, new { Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public async Task<RaportModel> GetRaportById(int Id)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<RaportModel>("SELECT * FROM Raports WHERE RaportId = @Id", new { Id });
                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task<FieldModel> GetFieldById(int Id)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<FieldModel>("SELECT * FROM Fields WHERE FieldId = @Id", new { Id });
                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public Task<bool> CreateField(FieldModel field)
        {
            throw new NotImplementedException();
        }

        public Task DeleteField(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
