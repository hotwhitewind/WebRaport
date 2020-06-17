using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebRaport.Models
{
    public class FieldModel
    {
        public int FieldId { get; set; }
        public string FieldTitle { get; set; }
        public string FromInfoTableName { get; set; }
        public string FromInfoColumnName { get; set; }
    }
}
