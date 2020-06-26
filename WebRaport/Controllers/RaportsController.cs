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
        public IActionResult Create()
        {
            return RedirectToAction("Index", "CreateRaport");
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