using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebRaport.Interfaces;
using WebRaport.ViewModels;

namespace WebRaport.Controllers
{
    public class LoginController : Controller
    {
        private IUserRepository _repository;

        public LoginController(IUserRepository repository)
        {
            _repository = repository;
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var users = await _repository.GetUsers();
            if (users == null || users.Count == 0)
            {
                return RedirectToAction("Index", "Register");
            } 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _repository.IsAuthentificate(viewModel.Login, viewModel.Password);
                if (user != null)
                {
                    if (user.Role != null)
                    {
                        await Authenticate(viewModel.Login, user.Role.Name);
                        return RedirectToAction("Index", "Home");
                    }

                    return View(viewModel);
                }
            }
            return View(viewModel);
        }

        private async Task Authenticate(string userName, string roleName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim("RequiredPermission", roleName),
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}
