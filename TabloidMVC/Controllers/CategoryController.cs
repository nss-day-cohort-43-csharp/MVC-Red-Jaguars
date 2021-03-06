﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: CategoryController
        public ActionResult Index()
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            List<Category> categories = _categoryRepository.GetAll();
            return View(categories);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            try
            {
                _categoryRepository.Add(category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            Category category = _categoryRepository.GetCategoryById(id);
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            try
            {
                _categoryRepository.Edit(category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            Category category = _categoryRepository.GetCategoryById(id);
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Category category)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            try
            {
                _categoryRepository.Delete(category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {

                return View(category);
            }
        }
    }
}
