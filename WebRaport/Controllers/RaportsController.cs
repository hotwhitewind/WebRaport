using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebRaport.Interfaces;
using WebRaport.Models;

namespace WebRaport.Controllers
{
    [Authorize(Policy = "EditorRequiredPermission")]
    public class RaportsController : Controller
    {
        private IRaportRepository _raportRepo;
        private IUserRepository _userRepository;
        private ILogger<RaportsController> _logger;

        public RaportsController(IRaportRepository raportRepository, ILogger<RaportsController> logger,
            IUserRepository userRepository)
        {
            _raportRepo = raportRepository;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var raports = await _raportRepo.GetCreatedRaports();
            foreach(var raport in raports)
            {
                raport.EditUserLogin = await _userRepository.GetLoginByUserId(raport.EditUserId);
            }
            return View(raports);
        }

        [NonAction]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                //Get data from database
                string currentUserName = this.ControllerContext.HttpContext.User.Identity.Name;
                var userIds = await _userRepository.GetUserIdByLoginName(currentUserName);
                if (userIds.Any())
                {
                    var currentRaportEdited = await _raportRepo.GetCreatingRaportByUserId(userIds.FirstOrDefault());
                    //_raportRepo
                    if (currentRaportEdited != null)
                    {
                        //данный пользователь создавал рапорт, вернем ему Id
                        return RedirectToAction("Index", "CreateRaport", new
                        {
                            @raportId = currentRaportEdited.RaportId,
                            @clearWorkDo = false
                        });
                    }
                    else
                    {
                        //создадим новый рапорт, Id не вернем
                        RaportModel newRaport = new RaportModel
                        {
                            EditUserId = userIds.FirstOrDefault()
                        };
                        if (!await _raportRepo.CreateRaport(newRaport))
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
                return RedirectToAction("Index", "CreateRaport");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Index", "CreateRaport");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, string pathToRaportFile = "")
        {
            var raport = await _raportRepo.GetRaportById(id);
            raport.RaportFilePath = pathToRaportFile;
            return View(raport);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RaportModel model)
        {
            string currentUserName = this.ControllerContext.HttpContext.User.Identity.Name;
            var userIds = await _userRepository.GetUserIdByLoginName(currentUserName);
            if (userIds.Any())
            {
                await _raportRepo.UpdateRaportTemplateFilePath(model.RaportId, model.RaportFilePath, userIds.FirstOrDefault());
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Home", "Error");
        }

        [NonAction]
        [HttpPost]
        public async Task<IActionResult> Delete(RaportModel raport)
        {
            await _raportRepo.DeleteRaport(raport.RaportId);
            return RedirectToAction("Index");
        }

        [NonAction]
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var raport = await _raportRepo.GetRaportById(Id);
            return PartialView("RemoveRaportViewPartial", raport);
        }
    }
}