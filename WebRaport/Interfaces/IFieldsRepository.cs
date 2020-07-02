using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Models;

namespace WebRaport.Interfaces
{
    public interface IFieldsRepository
    {
        public Task<List<FieldModel>> GetFields();
        public Task<List<FieldModel>> GetFieldsByRaportId(int RaportId);

        public Task<int> AddField(FieldModel field);
        public Task DeleteField(int FieldId);
        public Task RemoveAllFields();
        public Task RemoveAllNotRaportFields();
        public Task UpdateField(int FieldId);
        public Task<List<FieldTypesModel>> GetFieldTypes();
        public Task<List<CalculatedFieldTypesModel>> GetCalculatedFieldTypes();
        public Task<List<string>> GetTables();
        public Task<List<string>> GetTableColumns(string tableName);
    }
}
