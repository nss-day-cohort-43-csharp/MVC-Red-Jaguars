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
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }
        // GET: CommentController
        public IActionResult Index(int id)
        {
            List<Comment> comments = _commentRepository.GetCommentByPostId(id);
            return View(comments);
        }

        // GET: CommentController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentController/Create
        public IActionResult Create(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);
            CommentCreateViewModel vm = new CommentCreateViewModel()
            {
                Post = post,
                Comment = new Comment()
                {
                    PostId = post.Id,
                    UserProfileId = GetCurrentUserId()
                }
            };
            return View(vm);
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CommentCreateViewModel input)
        {
            try
            {
                input.Comment.CreateDateTime = DateTime.Now;
                _commentRepository.AddComment(input.Comment);
                return RedirectToAction(nameof(Index), new { id = input.Comment.PostId });
            }
            catch (Exception ex)
            {
                CommentCreateViewModel vm = new CommentCreateViewModel()
                {
                    Post = _postRepository.GetPublishedPostById(input.Comment.PostId),
                    Comment = input.Comment
                };
                vm.ErrorMessage = ex.ToString();
                return View(vm);
            }
        }

        // GET: CommentController/Edit/5
        public IActionResult Edit(int id)
        {
            Comment comment = _commentRepository.GetCommentById(id);
            if (GetCurrentUserId() == comment.UserProfileId)
            {
                return View(comment);
            }
            else
            {
                return RedirectToAction("Index", new { id = id });
            }
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Comment comment)
        {
            try
            {
                _commentRepository.UpdateComment(comment, id);
                return RedirectToAction(nameof(Index), new { id = comment.PostId });
            }
            catch (Exception ex)
            {

                return View(comment);
            }
        }

        // GET: CommentController/Delete/5
        public IActionResult Delete(int id)
        {
            Comment comment = _commentRepository.GetCommentById(id);
            if (GetCurrentUserId() == comment.UserProfileId || User.IsInRole("1"))
            {

                return View(comment);
            }
            else
            {
                return RedirectToAction("Index", new { id = id });
            }
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Comment comment)
        {
            try
            {
                Comment helper = _commentRepository.GetCommentById(id);
                int returnId = helper.PostId;
                _commentRepository.DeleteComment(id);
                return RedirectToAction(nameof(Index), nameof(Comment), new { id = returnId });
            }
            catch
            {
                return View(id);
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
