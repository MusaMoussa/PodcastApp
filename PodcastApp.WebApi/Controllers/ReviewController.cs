using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using PodcastApp.Data;
using PodcastApp.Models;
using PodcastApp.Services;

namespace PodcastApp.WebApi.Controllers
{
    [Authorize]
    public class ReviewController : ApiController
    {
        private ReviewService CreateReviewService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var reviewService = new ReviewService(userId);
            return reviewService;
        }

        /// <summary>
        /// Gets all the reviews
        /// </summary>
        /// <returns>ReviewListItem</returns>
        public IHttpActionResult Get()
        {
            ReviewService reviewService = CreateReviewService();
            var reviews = reviewService.GetReviewsForUser();
            return Ok(reviews);
        }

        // GET: api/Review/5
        /// <summary>
        /// Gets all the reviews for a specific podcast
        /// </summary>
        /// <returns>ReviewListItem</returns>
        public IHttpActionResult Get(int id)
        {
            ReviewService reviewService = CreateReviewService();
            var reviews = reviewService.GetReviewsByPodcastId(id);
            return Ok(reviews);
        }

        // POST: api/Review
        /// <summary>
        /// Adds a review to the database
        /// </summary>
        public IHttpActionResult Post(ReviewCreate review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateReviewService();

            if (!service.CreateReview(review))
                return InternalServerError();

            return Ok();
        }

        // PUT: api/Review/5
        /// <summary>
        /// Updates a review the database
        /// </summary>
        public IHttpActionResult Put(ReviewEdit review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateReviewService();

            if (!service.UpdateReview(review))
                return InternalServerError();

            return Ok();
        }

        // DELETE: api/Review/5
        /// <summary>
        /// Deletes a review from the database
        /// </summary>
        public IHttpActionResult Delete(int id)
        {
            var service = CreateReviewService();

            if (!service.DeleteReview(id))
                return InternalServerError();

            return Ok();
        }
    }
}
