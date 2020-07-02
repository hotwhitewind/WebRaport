using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GleamTech.FileUltimate.AspNet.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebRaport.Models;

namespace WebRaport.Pages
{
    public class FieldsTablePageModel : PageModel
    {
        public List<FieldModel> _fileds { get; set; }
        public FileManager fileManager { get; set; }
        public string PathToRaportFile { get; set; }

        public FieldsTablePageModel()
        {
            _fileds = new List<FieldModel>();
            fileManager = new FileManager
            {
                Height = 300,
                Resizable = true,
                ClientEvents = new FileManagerClientEvents()
                {
                    SelectionChanged = "fileSelected"
                }
            };
            var RootFolder = new FileManagerRootFolder
            {
                Location = "~/App_Data/Raports_Template",
                Name = "Шаблоны рапортов",
            };
            RootFolder.AccessControls.Add(new FileManagerAccessControl
            {
                Path = @"\",
                DeniedPermissions = FileManagerPermissions.Preview,
                AllowedPermissions = FileManagerPermissions.Full
            });
            fileManager.RootFolders.Add(RootFolder);
        }

        public void OnGet()
        {

        }

        public void OnGetAddField(FieldModel model)
        {

        }
    }
}