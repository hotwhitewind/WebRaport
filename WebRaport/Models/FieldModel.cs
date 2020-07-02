using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebRaport.Models
{
    
    public enum FieldTypesEnum
    { 
        DirectValue = 1, 
        CalculateValue = 5,
        AnotherTableSelectedRows = 6,
        UserDefinedValue = 7
    }

    public class FieldTypesModel
    {
        public int Id { get; set; }
        public string FieldType { get; set; }
        public string Description { get; set; }
    }

    public class CalculatedFieldTypesModel
    {
        public int Id { get; set; }
        public string CalculatedFieldType { get; set; }
        public string Description { get; set; }
    }

    public class FieldModel
    {        
        public int FieldId { get; set; }        
        public string FieldTitle { get; set; }
        public string FromInfoTableName { get; set; }
        public string FromInfoColumnName { get; set; }
        public bool FirstLetterUsing { get; set; }
        public string FieldDescription { get; set; }
        public int FieldType { get; set; }
        public string FieldDirectValue { get; set; }
        public int FieldCalculateType { get; set; }
    }
}
