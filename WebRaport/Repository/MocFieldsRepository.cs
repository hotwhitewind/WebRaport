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
    public class MocFieldsRepository : IFieldsRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MocFieldsRepository> _logger;

        public MocFieldsRepository(IConfiguration config, ILogger<MocFieldsRepository> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<int> AddField(FieldModel field)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
              "INSERT INTO Fields (FieldTitle,FromInfoTableName,FromInfoColumnName,FieldDescription,FieldType,FieldDirectValue," +
              "FieldCalculateType,FirstLetterUsing) " +
              "VALUES (@FieldTitle, @FromInfoTableName, @FromInfoColumnName, @FieldDescription,@FieldType,@FieldDirectValue," +
              "@FieldCalculateType,@FirstLetterUsing); " +
              "SELECT CAST(SCOPE_IDENTITY() as int)";
                    int? fieldIdRet = await db.QueryFirstOrDefaultAsync<int>(sqlQuery, field);
                    field.FieldId = fieldIdRet.Value;
                    return field.FieldId;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return -1;
                }
            }
        }

        public async Task DeleteField(int Id)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "DELETE FROM Fields WHERE FieldId = @Id;";
                    await db.ExecuteAsync(sqlQuery, new { Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public async Task<List<FieldModel>> GetFields()
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<FieldModel>("SELECT * FROM Fields");

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task RemoveAllNotRaportFields()
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "DELETE FROM Fields WHERE RaportFlag = 0;";
                    await db.ExecuteAsync(sqlQuery);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public async Task UpdateField(int FieldId)
        {
        }

        public async Task RemoveAllFields()
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var sqlQuery =
                        "DELETE * FROM Fields;";
                    await db.ExecuteAsync(sqlQuery);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public async Task<List<FieldTypesModel>> GetFieldTypes()
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<FieldTypesModel>("SELECT * FROM FieldTypes");

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task<List<CalculatedFieldTypesModel>> GetCalculatedFieldTypes()
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<CalculatedFieldTypesModel>("SELECT * FROM CalculatedFieldTypes");

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task<List<string>> GetTables()
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<string>("SELECT name FROM reportDB.sys.Tables");

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task<List<string>> GetTableColumns(string tableName)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<string>("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " +
                        "WHERE TABLE_NAME = @tableName", new { tableName });

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task<List<FieldModel>> GetFieldsByRaportId(int RaportId)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DBConnectionString")))
            {
                try
                {
                    var result = await db.QueryAsync<FieldModel>("SELECT * FROM RaportFields WHERE RaportId = @RaportId",
                        new { RaportId });
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
