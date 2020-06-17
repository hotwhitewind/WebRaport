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

        private ILogger<RaportsController> _logger;

        public RaportsController(IRaportRepository raportRepository, ILogger<RaportsController> logger)
        {
            _raportRepo = raportRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var raports = await _raportRepo.GetRaports();
            return View(raports);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            using(var file = new FileStream(@"c:\Project\WebRaport\WebRaport\wwwroot\App_Data\ExampleFiles\Default.docx", FileMode.Open, FileAccess.Read))
            {
                byte[] newBuff = new byte[file.Length];
                await file.ReadAsync(newBuff, 0, (int)file.Length);
                var newRaport = new RaportModel();
                newRaport.RaportTitle = "test";
                newRaport.RaportData = newBuff;
                await _raportRepo.CreateRaport(newRaport);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RaportModel raport)
        {
            await _raportRepo.DeleteRaport(raport.RaportId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var raport = await _raportRepo.GetRaportById(Id);
            return PartialView("RemoveRaportViewPartial", raport);
        }
    }
}