﻿using Microsoft.AspNetCore.Authorization;
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
            catch
            {
                CommentCreateViewModel vm = new CommentCreateViewModel()
                {
                    Post = _postRepository.GetPublishedPostById(input.Comment.PostId),
                    Comment = input.Comment
                };
                vm.ErrorMessage = "Woops! Something went wrong while saving this comment.";
                return View(vm);
            }
        }

        // GET: CommentController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentController/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
