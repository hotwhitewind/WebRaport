using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebRaport.Models
{
    public class RaportModel
    {
        public int RaportID { get; set; }
        public string RaportTitle { get; set; }
        public List<FieldModel> Fields { get; set; }
    }
}
