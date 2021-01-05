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

        public UserController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public IActionResult Index()
        {
            var users = _userProfileRepository.GetAllUsers().OrderBy(user => user.DisplayName);
            return View(users);
        }
        public IActionResult DeactiveView()
        {
            var users = _userProfileRepository.GetAllDeactiveUsers().OrderBy(user => user.DisplayName);
            return View(users);
        }
        public IActionResult Details(int id)
        {
            var user = _userProfileRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        public ActionResult Deactivate(int id)
        {
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

    }
}
