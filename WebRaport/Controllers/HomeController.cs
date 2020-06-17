using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using WebRaport.Interfaces;
using WebRaport.Models;
using WebRaport.ViewModels;

namespace WebRaport.Controllers
{
    public class HomeController : Controller
    {
        private IUserRepository _userRepo;
        private IRaportRepository _raportRepo;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IRaportRepository raportRepository)
        {
            _logger = logger;
            _userRepo = userRepository;
            _raportRepo = raportRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.GetUsers();
            var raports = await _raportRepo.GetRaports();
            var usersRaportsViewModel = new UsersAndRaportsViewModel();

            usersRaportsViewModel.UsersList = users.
                Select(a => new SelectListItem()
                {
                    Value = a.UserId.ToString(),
                    Text = $"{a.LastName} {a.FirstName} {a.SecondName}"
                }).
                ToList();
            usersRaportsViewModel.RaportsList = raports.
                Select(a => new SelectListItem()
                {
                    Value = a.RaportId.ToString(),
                    Text = $"{a.RaportTitle}"
                }).
                ToList();

            return View(usersRaportsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(UsersAndRaportsViewModel userRaportViewModel)
        {
            return RedirectToAction("Index", "RaportOptions", userRaportViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
