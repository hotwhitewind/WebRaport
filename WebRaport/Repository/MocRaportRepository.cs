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
                        "INSERT INTO Raports (RaportTitle, RaportFilePath, IsCreated, EditUserId) " +
                        "VALUES (@RaportTitle, @RaportFilePath, @IsCreated, @EditUserId); " +
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

        public async Task<bool> AddFieldIntoRaport(int RaportId, int FieldId)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "INSERT INTO RaportFields (RaportId, FieldId) " +
                        "VALUES (@RaportId, @FieldId); " +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";
                    int? raportIdRet = await db.QueryFirstOrDefaultAsync<int>(sqlQuery, new { RaportId, FieldId });

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return false;
                }
            }
        }

        public async Task SetRaportCreated(int RaportId)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "UPDATE Raports SET IsCreated = 1 WHERE RaportId = @RaportId";
                    await db.ExecuteAsync(sqlQuery, new { RaportId });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public async Task UpdateRaport(RaportModel raport)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "UPDATE Raports SET RaportTitle = @RaportTitle, RaportFilePath = @RaportFilePath, " +
                        "IsCreated = @IsCreated, EditUserId = @EditUserId WHERE RaportId = @RaportId";
                    await db.ExecuteAsync(sqlQuery, raport);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public async Task<RaportModel> GetCreatingRaportByUserId(int UserId)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<RaportModel>("SELECT * FROM Raports WHERE " +
                        "CreatingUserId = @UserId AND IsCreated = 0", new { UserId });
                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task<List<RaportModel>> GetCreatedRaports()
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<RaportModel>("SELECT * FROM Raports WHERE IsCreated = 1");

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task UpdateRaportTemplateFilePath(int RaportId, string path, int editUserId)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "UPDATE Raports SET RaportFilePath = @RaportFilePath, EditUserId = @EditUserId " +
                        "WHERE RaportId = @RaportId";
                    await db.ExecuteAsync(sqlQuery, new { RaportId, RaportFilePath = path, EditUserId = editUserId });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public async Task<string> GetHandlerPageName(int RaportId)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<string>("SELECT RaportHandlerPageName FROM Raports " +
                        "WHERE RaportId = @RaportId", new { RaportId });

                    return result.FirstOrDefault();
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
