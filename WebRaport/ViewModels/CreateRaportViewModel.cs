using GleamTech.FileUltimate.AspNet.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRaport.Models;

namespace WebRaport.ViewModels
{
    public class SetPathToRaportViewModel
    {
        public int RaportId { get; set; }
        public FileManager fileManager { get; set; }
        public string PathToRaportFile { get; set; }
    }

    public class CreateRaportViewModel
    {
        public int EditedRaportId { get; set; }
        public string PathToRaportFile { get; set; }
    }
}
