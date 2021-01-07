using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using System.Linq;
using TabloidMVC.Models;
using System;

namespace TabloidMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserTypeRepository _userTypeRepository;

        public UserController(IUserProfileRepository userProfileRepository, IUserTypeRepository userTypeRepository)
        {
            _userProfileRepository = userProfileRepository;
            _userTypeRepository = userTypeRepository;
        }

        public IActionResult Index()
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            var users = _userProfileRepository.GetAllUsers().OrderBy(user => user.DisplayName);
            return View(users);
        }
        public IActionResult DeactiveView()
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            var users = _userProfileRepository.GetAllDeactiveUsers().OrderBy(user => user.DisplayName);
            return View(users);
        }
        public IActionResult Details(int id)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            var user = _userProfileRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        public ActionResult Deactivate(int id)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            DeactivateUserViewModel vm = new DeactivateUserViewModel()
            {
                User = _userProfileRepository.GetUserById(id)
            };
            if (vm.User == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(int id, DeactivateUserViewModel vm)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            vm = new DeactivateUserViewModel()
            {
                User = _userProfileRepository.GetUserById(id)
            };
            try
            {
                _userProfileRepository.DeactivateUser(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                vm.ErrorMsg = ex.Message;
                return View(vm);
            }
        }
        public ActionResult Activate(int id)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            var user = _userProfileRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(int id, UserProfile user)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            try
            {
                _userProfileRepository.ActivateUser(id);

                return RedirectToAction("Index");
            }
            catch
            {

                return View(user);
            }
        }

        public ActionResult ChangeType(int id)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            var types = _userTypeRepository.GetUserTypes();
            var user = _userProfileRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            var vm = new UserProfileTypeViewModel()
            {
                olduser = user,
                user = user,
                type = types
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeType(UserProfileTypeViewModel vm, int id)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            try
            {
                int Admin = _userProfileRepository.AdminCount();
                if (vm.olduser.UserTypeId == 1 && Admin >= 2 || vm.olduser.UserTypeId != 1)
                {

                    _userProfileRepository.ChangeUserType(vm.user);

                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception("Need to always have at least one admin!");
                }
            }
            catch (Exception ex)
            {
                vm.ErrorMsg = ex.Message;
                vm.user = _userProfileRepository.GetUserById(vm.user.Id);
                vm.type = _userTypeRepository.GetUserTypes();
                return View(vm);
            }
        }

    }
}
