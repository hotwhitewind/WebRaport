using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Models;

namespace WebRaport.Interfaces
{
    interface IReportRepository
    {
        Task<List<ReportModel>> GetReports();
        Task<List<FieldModel>> GetFieldsByReportId(int Id);
    }
}
