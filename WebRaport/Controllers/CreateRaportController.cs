using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GleamTech.FileUltimate.AspNet.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebRaport.Interfaces;
using WebRaport.Models;
using WebRaport.ViewModels;

namespace WebRaport.Controllers
{
    [Authorize(Policy = "EditorRequiredPermission")]
    public class CreateRaportController : Controller
    {
        private IRaportRepository _raportRepo;
        private CreateRaportViewModel _newRaport;
        private ILogger<CreateRaportController> _logger;
        private IFieldsRepository _fieldRepository;

        public CreateRaportController(IRaportRepository raportRepository, ILogger<CreateRaportController> logger, 
            IFieldsRepository fieldsRepository)
        {
            _raportRepo = raportRepository;
            _logger = logger;
            _fieldRepository = fieldsRepository;
        }

        public IActionResult Index()
        {
            _newRaport = new CreateRaportViewModel();
            var fileManager = new FileManager
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
            _newRaport.fileManager = fileManager;
            _newRaport.raportFileds = new List<FieldModel>();
            return View(_newRaport);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaportViewModel raport)
        {
            return RedirectToAction("Index", "Raports");
        }

        [HttpPost]
        public async Task<JsonResult> AddField(FieldModel newField)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Form is not valid! " +
                      "Please correct it and try again."
                    });
                }
                //int newFieldId = await _fieldRepository.AddField(newField);
                //newField.FieldId = newFieldId;
                return Json(new { Result = "OK", Record = newField });
            }
            catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteField(int fieldId)
        {
            try
            {
                await _fieldRepository.DeleteField(fieldId);
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetTablesOptions(int FieldType = 0)
        {
            if (FieldType == (int)FieldTypesEnum.AnotherTableSelectedRows)
            {
                try
                {
                    var tables = await _fieldRepository.GetTables();
                    return Json(new { Result = "OK", Options = tables });
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "ERROR", Message = ex.Message });
                }
            }
            else
            {
                return Json(new { Result = "OK", Options = new List<string>() });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetTableColumnsOptions(string fromInfoTableName = "")
        {
            try
            {
                var tableColumns = await _fieldRepository.GetTableColumns(fromInfoTableName);
                return Json(new { Result = "OK", Options = tableColumns });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetFieldTypeOptions()
        {
            try
            {
                var fieldTypes = await _fieldRepository.GetFieldTypes();
                var types = fieldTypes.Select(c => new { DisplayText = c.Description, Value = c.Id });
                return Json(new { Result = "OK", Options = types });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetCalculatedFieldTypeOptions()
        {
            try
            {
                var fieldTypes = await _fieldRepository.GetCalculatedFieldTypes();
                var types = fieldTypes.Select(c => new { DisplayText = c.Description, Value = c.Id });
                return Json(new { Result = "OK", Options = types });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> FieldsList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //Get data from database
                List<FieldModel> fields = await _fieldRepository.GetCreatedFields();
                if(fields != null)
                    return Json(new { Result = "OK", Records = fields, TotalRecordCount = fields.Count });
                else
                    return Json(new { Result = "OK"});
            }
            catch (Exception ex)
            {
                return Json (new  
                { 
                    Result = "ERROR", 
                    Message = ex.Message 
                });
            }
        }
    }
}