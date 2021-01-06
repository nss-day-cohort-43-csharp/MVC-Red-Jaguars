using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IPostRepository _postRepository;

        public HomeController(ILogger<HomeController> logger, ISubscriptionRepository subscriptionRepository, IPostRepository postRepository)
        {
            _logger = logger;
            _subscriptionRepository = subscriptionRepository;
            _postRepository = postRepository;
        }

        public IActionResult Index()
        {
            try
            {
                int loggedInUser = GetCurrentUserProfileId();

                List<Subscription> userSubscriptions = _subscriptionRepository.GetUserSubscriptions(loggedInUser);

                List<Post> postsToView = new List<Post>();

                foreach (Subscription subscription in userSubscriptions)
                {
                    List<Post> providerPosts = _postRepository.GetPostsByUser(subscription.ProviderUserProfileId);

                    foreach (Post post in providerPosts)
                    {
                        postsToView.Add(post);
                    }
                }


                return View(postsToView);

            }
            catch
            {
                return View();
            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
