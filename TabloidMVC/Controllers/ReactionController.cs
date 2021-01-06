using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class ReactionController : Controller
    {
        private readonly IPostReactionRepository _postReactionRepository;
        private readonly IReactionRepository _reactionRepository;

        public ReactionController(IPostReactionRepository postReactionRepository, IReactionRepository reactionRepository)
        {
            _postReactionRepository = postReactionRepository;
            _reactionRepository = reactionRepository;
        }
        public ActionResult React(int postId, int reactionId)
        {
            PostReaction postReaction = new PostReaction();
            postReaction.PostId = postId;
            postReaction.ReactionId = reactionId;
            postReaction.UserProfileId = GetCurrentUserId();

            _postReactionRepository.AddPostReaction(postReaction);

            return RedirectToAction("Details", "Post", new { id = postId });

        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
