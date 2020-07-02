using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GleamTech.FileUltimate.AspNet.UI;
using Microsoft.AspNetCore.Mvc;
using WebRaport.ViewModels;

namespace WebRaport.Controllers
{
    public class FileManagerController : Controller
    {
        public IActionResult Index(int id)
        {
            var fileManager = new FileManager
            {
                Height = 500,
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
            SetPathToRaportViewModel _newRaport = new SetPathToRaportViewModel();
            _newRaport.fileManager = fileManager;
            _newRaport.RaportId = id;
            return View(_newRaport);
        }

        public IActionResult SetPathToRaport(SetPathToRaportViewModel model)
        {
            return RedirectToAction("Edit", "Raports", new
            {
                @pathToRaportFile = model.PathToRaportFile,
                @id = model.RaportId
            });            
        }

        public IActionResult BackToRaportCreate(int id)
        {
            return RedirectToAction("Edit", "Raports", new { id });
        }
    }
}