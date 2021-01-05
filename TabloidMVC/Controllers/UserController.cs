﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using System.Linq;
using TabloidMVC.Models;

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
            var user = _userProfileRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(int id, UserProfile user)
        {
            try
            {
                _userProfileRepository.DeactivateUser(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(user);
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