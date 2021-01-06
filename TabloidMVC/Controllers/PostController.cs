using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using System.Linq;
using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
<<<<<<< HEAD
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
=======
        private readonly ISubscriptionRepository _subscriptionRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ISubscriptionRepository subscriptionRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _subscriptionRepository = subscriptionRepository;
>>>>>>> master
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts().OrderByDescending(post => post.PublishDateTime);
            return View(posts);
        }

        public IActionResult MyIndex()
        {
            var posts = _postRepository.GetPostsByUser(GetCurrentUserProfileId()).OrderByDescending(post => post.CreateDateTime);
            return View(posts);
        }

        public IActionResult Details(int id)
        {
<<<<<<< HEAD
            PostTagsViewModel viewModel = new PostTagsViewModel
            {
                post = _postRepository.GetPublishedPostById(id),
                tags = _tagRepository.GetTagPostById(id)

            };

            if (viewModel.post == null)
            {
                int userId = GetCurrentUserProfileId();
                viewModel.post = _postRepository.GetUserPostById(id, userId);
                if (viewModel.post == null)
=======
            var post = _postRepository.GetPublishedPostById(id);
            int userId = GetCurrentUserProfileId();

            if (post == null)
            {                
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
>>>>>>> master
                {
                    return NotFound();
                }
            }
<<<<<<< HEAD
            return View(viewModel);
=======

            List<Subscription> mySubscriptions = _subscriptionRepository.GetUserSubscriptions(userId);

            PostDetailsViewModel vm = new PostDetailsViewModel();

            vm.Post = post;

            foreach (Subscription subscription in mySubscriptions)
            {
                if (subscription.ProviderUserProfileId == post.UserProfileId)
                {
                    vm.Subscribed = true;
                }
            }

            return View(vm);
>>>>>>> master
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }
        public ActionResult Edit(int id)
        {
            var post = _postRepository.GetUserPostById(id, GetCurrentUserProfileId());

            if (post == null)
            {
                return NotFound();
            }

            else
            {
                var vm = new PostCreateViewModel();
                vm.CategoryOptions = _categoryRepository.GetAll();
                vm.Post = post;

                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
                _postRepository.UpdatePost(post, id);

                return RedirectToAction("Details", new { id = id });
            }
            catch
            {
                return RedirectToAction("Edit", id);
            }
        }

        public ActionResult Delete(int id)
        {
            var post = _postRepository.GetUserPostById(id, GetCurrentUserProfileId());

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(post);
            }
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
