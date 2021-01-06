using Microsoft.AspNetCore.Authorization;
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
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        // GET: TagController
        public ActionResult Index()
        {
            List<Tag> tags = _tagRepository.GetAllTags();

            return View(tags);
        }

        // GET: TagController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TagController/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            try
            {
                _tagRepository.AddTag(tag);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }

        // GET: TagController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            Tag tag = _tagRepository.GetTagById(id);

            return View(tag);
        }

        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tag tag)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            try
            {
                _tagRepository.UpdateTag(tag);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }

        // GET: TagController/Delete/5
        public ActionResult Delete(int id)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            Tag tag = _tagRepository.GetTagById(id);

            return View(tag);
        }

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tag tag)
        {
            if (!User.IsInRole("1")) { return RedirectToAction("Index", "Home"); }
            try
            {
                _tagRepository.DeleteTag(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }
    }
}
