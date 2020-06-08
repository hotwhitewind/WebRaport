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
    }
}
