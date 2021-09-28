using Microsoft.AspNet.Identity;
using PodcastApp.Models;
using PodcastApp.Services;
using System;
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
        /// <summary>
        /// Get all subscriptions for a user
        /// </summary>
        /// /// <returns>SubscriptionListItem</returns>
        public IHttpActionResult Get()
        {
            SubscriptionService subscriptionService = CreateSubscriptionService();
            var subscriptions = subscriptionService.GetSubscriptions();
            return Ok(subscriptions);
        }

        // GET: api/Subscription/5
        /// <summary>
        /// Get a subscription by id
        /// </summary>
        /// /// <returns>SubscriptionDetail</returns>
        public IHttpActionResult Get(int id)
        {
            SubscriptionService subscriptionService = CreateSubscriptionService();
            var subscription = subscriptionService.GetSubscriptionById(id);

            if (subscription == null)
            {
                return NotFound();
            }
            return Ok(subscription);
        }

        // POST: api/Subscription
        /// <summary>
        /// Create a new subscription to a podcast
        /// </summary>
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
        /// <summary>
        /// Update a subscription to a podcast
        /// </summary>
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
        /// <summary>
        /// Delete a subscription to a podcast
        /// </summary>
        public IHttpActionResult Delete(int id)
        {
            var service = CreateSubscriptionService();

            if (!service.DeleteSubscription(id))
                return InternalServerError();
            return Ok();
        }
    }
}
