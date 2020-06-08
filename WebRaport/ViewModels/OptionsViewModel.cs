using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Models;

namespace WebRaport.ViewModels
{
    public class OptionsViewModel
    {
        public List<FiledModelWithValue> fieldModelsWithValue { get; set; }
        public User userInfo { get; set; }        
    }
}
