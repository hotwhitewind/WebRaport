using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GleamTech.FileUltimate.AspNet.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebRaport.Interfaces;
using WebRaport.Models;
using WebRaport.ViewModels;

namespace WebRaport.Controllers
{
    [NonController]
    [Authorize(Policy = "EditorRequiredPermission")]
    public class CreateRaportController : Controller
    {
        private IRaportRepository _raportRepo;
        private ILogger<CreateRaportController> _logger;
        private IFieldsRepository _fieldRepository;
        private IUserRepository _userRepository;

        public CreateRaportController(IRaportRepository raportRepository, ILogger<CreateRaportController> logger, 
            IFieldsRepository fieldsRepository, IUserRepository userRepository)
        {
            _raportRepo = raportRepository;
            _logger = logger;
            _fieldRepository = fieldsRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index(int? raportId, bool clearWorkDo = false, string pathToRaportFile = "")
        {
            if (!clearWorkDo)
            {
                //сюда мы придем из Raport/Create
                CreateRaportViewModel _newRaport = new CreateRaportViewModel();
                _newRaport.EditedRaportId = raportId.GetValueOrDefault();
                _newRaport.PathToRaportFile = pathToRaportFile;
                return View(_newRaport);
            }
            else
            {
                //сюда мы придем из CreateRaport/ResetCurrentWork
                //создадим новый рапорт, Id не вернем
                string currentUserName = this.ControllerContext.HttpContext.User.Identity.Name;
                var userIds = await _userRepository.GetUserIdByLoginName(currentUserName);
                if (userIds.Any())
                {
                    RaportModel newRaport = new RaportModel
                    {
                        EditUserId = userIds.FirstOrDefault()
                    };
                    if (!await _raportRepo.CreateRaport(newRaport))
                    {
                        CreateRaportViewModel _newRaport = new CreateRaportViewModel();
                        _newRaport.EditedRaportId = 0;
                        _newRaport.PathToRaportFile = "";
                        return View(_newRaport);
                    }
                }
                //было бы неплохо сделать редирект на страницу с ошибкой
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetCurrentWork(CreateRaportViewModel model)
        {
            //удаляем текущую работу
            await _raportRepo.DeleteRaport(model.EditedRaportId);
            return RedirectToAction("Index", new
            {
                @clearWorkDo = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaportViewModel raport)
        {
            return RedirectToAction("Index", "Raports");
        }

        [HttpPost]
        public JsonResult AddField(FieldModel newField)
        {
            try
            {
                return Json(new { Result = "ERROR", Message = "Not TODO" });
                //return Json(new { Result = "OK", Record = newField });
            }
            catch (Exception ex)
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
        public async Task<JsonResult> GetCalculatedFieldTypeOptions(int FieldType = 0)
        {
            if (FieldType == (int)FieldTypesEnum.CalculateValue)
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
            else
            {
                return Json(new { Result = "OK", Options = new List<string>() });
            }
        }

        public IActionResult FieldsTablePartial(List<FieldModel> fields)
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<JsonResult> FieldsList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //Get data from database
                string currentUserName = this.ControllerContext.HttpContext.User.Identity.Name;
                var userIds = await _userRepository.GetUserIdByLoginName(currentUserName);
                if (userIds.Any())
                {
                    var currentRaportEdited = await _raportRepo.GetCreatingRaportByUserId(userIds.FirstOrDefault());
                    if (currentRaportEdited != null)
                    {
                        List<FieldModel> fields = await _fieldRepository.GetFieldsByRaportId(currentRaportEdited.RaportId);
                        if (fields != null)
                            return Json(new { Result = "OK", Records = fields, TotalRecordCount = fields.Count });
                    }
                }
                return Json(new { Result = "OK" });
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