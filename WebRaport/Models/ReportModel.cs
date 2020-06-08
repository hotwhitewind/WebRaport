using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebRaport.Models
{
    public class ReportModel
    {
        public int Id { get; set; }
        public string ReportTitle { get; set; }
        public List<FieldModel> Fields { get; set; }
    }
}
