using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IPostRepository _postRepository;
        public TagController(ITagRepository tagRepository, IPostRepository postRepository)
        {
            _tagRepository = tagRepository;
            _postRepository = postRepository;
        }

        // GET: TagController
        public ActionResult Index()
        {
            if(GetCurrentUserType() != 1)
            {
                return NotFound();
            }
            else
            {
                List<Tag> tags = _tagRepository.GetAllTags();

                return View(tags);
            }
            
        }

        // GET: TagController/Details/5
        public ActionResult Details(int id)
        {
            if (GetCurrentUserType() != 1)
            {
                return NotFound();
            }
            else
            {
                return View();
            }
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

        public ActionResult GetTagsForPost(int id)
        {

            PostDetailsViewModel vm = new PostDetailsViewModel()
            {
                Tags = _tagRepository.GetAllTags(),
                Post = _postRepository.GetPublishedPostById(id)
            };
            if (User.IsInRole("1")) { return View(vm); }
            else if (GetCurrentUserId() == vm.Post.UserProfileId) { return View(vm); }
            else { return RedirectToAction("Index", "Home"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTagsForPost(int id, PostDetailsViewModel viewModel)
        {
            try
            {
                _tagRepository.AddTagToPost(viewModel.Tag.Id, viewModel.Post);

                return RedirectToAction(nameof(Details), "Post", new { id = viewModel.Post.Id });
            }
            catch (Exception ex)
            {
                return View(viewModel);
            }
        }

        public ActionResult GetTagsFromPostDetails(int id)
        {
            PostDetailsViewModel vm = new PostDetailsViewModel()
            {
                Tags = _tagRepository.GetTagForDelete(id),
                Post = _postRepository.GetPublishedPostById(id)
            };
            if (User.IsInRole("1")) { return View(vm); }
            else if (GetCurrentUserId() == vm.Post.UserProfileId) { return View(vm); }
            else { return RedirectToAction("Index", "Home"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTagsFromPostDetails(int id, PostDetailsViewModel viewModel)
        {
            try
            {
                _tagRepository.DeleteTagFromPost(viewModel.Tag.Id);

                return RedirectToAction(nameof(Details), "Post", new { id = viewModel.Post.Id });
            }
            catch (Exception ex)
            {
                return View(viewModel);
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        private int GetCurrentUserType()
        {
            string id = User.FindFirstValue(ClaimTypes.Role);
            return int.Parse(id);
        }
    }
}
