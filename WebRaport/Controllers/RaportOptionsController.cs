using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebRaport.Interfaces;
using WebRaport.Models;
using WebRaport.ViewModels;

namespace WebRaport.Controllers
{
    public class RaportOptionsController : Controller
    {
        private IUsersRepository _userRepo;
        private IRaportRepository _raportRepo;

        private ILogger<RaportOptionsController> _logger;

        public RaportOptionsController(ILogger<RaportOptionsController> logger, IUsersRepository userRepository, IRaportRepository raportRepository)
        {
            _logger = logger;
            _userRepo = userRepository;
            _raportRepo = raportRepository;
        }

        public async Task<IActionResult> Index(UsersAndRaportsViewModel userRaportViewModel)
        {
            var filedsList = await _raportRepo.GetFieldsByRaportId(userRaportViewModel.RaportId);
            var fieldsWithValueList = new List<FiledModelWithValue>();
            foreach(var field in filedsList)
            {
                switch(field.FromInfoTableName)
                {
                    case "Users":
                        var value = await _userRepo.GetUserFiledValueByColumnName(userRaportViewModel.UserId, field.FromInfoColumnName);
                        var newFieldWithValue = new FiledModelWithValue
                        {
                            FieldName = field.FieldTitle,
                            FieldValue = value
                        };
                        fieldsWithValueList.Add(newFieldWithValue);
                        break;
                }
            }
            return View(fieldsWithValueList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Preview(List<FiledModelWithValue> fieldsModelWithView)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}