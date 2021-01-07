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
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }
        
        // GET: SubscriptionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SubscriptionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubscriptionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubscriptionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: SubscriptionController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: SubscriptionController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: SubscriptionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubscriptionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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

        public ActionResult Subscribe(int providerId, int postId)
        {
            Subscription subscription = new Subscription();

            subscription.ProviderUserProfileId = providerId;
            subscription.SubscriberUserProfileId = GetCurrentUserId();
            subscription.BeginDateTime = DateTime.Now;
            subscription.EndDateTime = DateTime.MaxValue;

            _subscriptionRepository.Add(subscription);

            return RedirectToAction("Details", "Post", new { id = postId });

        }

        public ActionResult Unsubscribe(int subId, int postId)
        {
            Subscription subscription = _subscriptionRepository.GetSubscriptionById(subId);

            _subscriptionRepository.Edit(subscription);

            return RedirectToAction("Details", "Post", new { id = postId });
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
