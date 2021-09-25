using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        // GET: api/Review/5
        public IHttpActionResult Get()
        {
            ReviewService reviewService = CreateReviewService();
            var reviews = reviewService.GetReviews();
            return Ok(reviews);
        }

        public IHttpActionResult Get(int id)
        {
            ReviewService reviewService = CreateReviewService();
            var reviews = reviewService.GetReviewsByPodcastId(id);
            return Ok(reviews);
        }

        // POST: api/Review
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
        public IHttpActionResult Delete(int id)
        {
            var service = CreateReviewService();

            if (!service.DeleteReview(id))
                return InternalServerError();

            return Ok();
        }
    }
}
