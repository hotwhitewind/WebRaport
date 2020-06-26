using GleamTech.FileUltimate.AspNet.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Models;

namespace WebRaport.ViewModels
{
    public class CreateRaportViewModel
    {
        public FileManager fileManager { get; set; }
        public List<FieldModel> raportFileds { get; set; }
        public string PathToRaportFile { get; set; }
    }
}
