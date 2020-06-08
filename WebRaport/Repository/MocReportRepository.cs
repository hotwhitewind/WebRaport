using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Interfaces;
using WebRaport.Models;

namespace WebRaport.Repository
{
    public class MocReportRepository : IReportRepository
    {
        private readonly IConfiguration _config;

        public MocReportRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<List<ReportModel>> GetReports()
        {
            throw new NotImplementedException();
        }

        public Task<List<FieldModel>> GetFieldsByReportId(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
