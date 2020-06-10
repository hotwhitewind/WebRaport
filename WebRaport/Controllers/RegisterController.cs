using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebRaport.Interfaces;
using WebRaport.Models;

namespace WebRaport.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPermissionRepository _permissionRepository;

        public RegisterController(IUserRepository userRepository, IPermissionRepository permissionRepository)
        {
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AdminRegisterModel adminRegisterModel)
        {
            var checkPermission = await _permissionRepository.Get("admin");
            if (checkPermission == null)
            {
                var adminPermission = new Permission()
                {
                    Name = "admin",
                    Description = "Administrative permission",
                    PermissionID = 1
                };

                await _permissionRepository.Create(adminPermission);
                checkPermission = adminPermission;
            }

            var newAdmin = new User
            {
                Password = adminRegisterModel.Password,
                Login = adminRegisterModel.Login,
                Role = checkPermission,
                BirthDay = DateTime.Now
            };
            await _userRepository.Create(newAdmin);
            return RedirectToAction("Login", "Login");
        }
    }
}