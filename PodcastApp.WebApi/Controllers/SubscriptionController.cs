using Microsoft.AspNet.Identity;
using PodcastApp.Data;
using PodcastApp.Models;
using PodcastApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PodcastApp.WebApi.Controllers
{
    [Authorize]
    public class SubscriptionController : ApiController
    {
        private SubscriptionService CreateSubscriptionService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var subscriptionService = new SubscriptionService(userId);
            return subscriptionService;
        }
        
        // GET: api/Subscription
        public IHttpActionResult Get()
        {
            SubscriptionService subscriptionService = CreateSubscriptionService();
            var subscriptions = subscriptionService.GetSubscriptions();
            return Ok(subscriptions);
        }

        // GET: api/Subscription/5
        public IHttpActionResult Get (int id)
        {
            SubscriptionService subscriptionService = CreateSubscriptionService();
            var subscription = subscriptionService.GetSubscriptionById(id);
            return Ok(subscription);
        }

        // POST: api/Subscription
        public IHttpActionResult Post(SubscriptionCreate subscription)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateSubscriptionService();

            if (!service.CreateSubscription(subscription))
                return InternalServerError();

            return Ok();
        }

        // PUT: api/Subscription/5
        public IHttpActionResult Put(SubscriptionEdit subscription)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateSubscriptionService();

            if (!service.UpdateSubscription(subscription))
                return InternalServerError();

            return Ok();
        }

        // DELETE: api/Subscription/5
        public IHttpActionResult Delete(int id)
        {
            var service = CreateSubscriptionService();

            if (!service.DeleteSubscription(id))
                return InternalServerError();
            return Ok();
        }
    }
}
