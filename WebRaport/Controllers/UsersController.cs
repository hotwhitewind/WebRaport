using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebRaport.Interfaces;
using WebRaport.Models;
using WebRaport.ViewModels;

namespace WebRaport.Controllers
{
    [Authorize(Policy = "AdminRequiredPermission")]
    public class UsersController : Controller
    {
        private IUserRepository _userRepository;
        private IPermissionRepository _permissionRepository;

        public UsersController(IUserRepository userRepository, IPermissionRepository permissionRepository)
        {
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetUsers();
            return View(users);
        }

        [Authorize(Policy = "EditorRequiredPermission")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            UserCreateViewModel newUser = new UserCreateViewModel();
            newUser.AllRoles = await _permissionRepository.GetPermissions();
            return View(newUser);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel user)
        {
            User newUser = new User()
            {
                Login = user.Login,
                Password =  user.Password,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastName = user.LastName,
                BirthDay = user.BirthDay,
                Rank = user.Rank,
                Position = user.Position,
                PersonalNumber = user.PersonalNumber
            };

            await _userRepository.Create(newUser);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserPassword(int Id)
        {
            User user = await _userRepository.Get(Id);
            if (user == null)
            {
                return NotFound();
            }

            ChangePasswordViewModel model = new ChangePasswordViewModel()
            {
                id = user.UserID,
                Login = user.Login,
                PasswordConfirm = "",
                NewPassword = ""
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPassword(ChangePasswordViewModel password)
        {
            User user = await _userRepository.Get(password.id);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.UpdatePassword(user.UserID, password.NewPassword);
            return RedirectToAction("Edit", new {Id = password.id});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var editUser = await _userRepository.Get(Id);
            editUser.Role = await _permissionRepository.GetByUserId(editUser.UserID);
            return View(editUser);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            await _userRepository.Update(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserRole(int Id)
        {
            var user = await _userRepository.Get(Id);
            if (user == null)
            {
                return NotFound();
            }

            user.Role = await _permissionRepository.GetByUserId(user.UserID);

            ChangeUserRoleViewModel model = new ChangeUserRoleViewModel()
            {
                Id = user.UserID,
                Login = user.Login,
                CurrentRole = user.Role.Name,
                LastName = user.LastName,
                FirstName = user.FirstName,
                selectRoles = await _permissionRepository.GetPermissions()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(ChangeUserRoleViewModel model)
        {
            await _permissionRepository.ChangePermissionForUserByName(model.Id, model.CurrentRole);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(User user)
        {
            await _userRepository.Delete(user.UserID);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var user = await _userRepository.Get(Id);
            return PartialView("RemoveUserViewPartial", user);
        }
    }
}