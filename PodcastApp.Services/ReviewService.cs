using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PodcastApp.Data;
using PodcastApp.Models;

namespace PodcastApp.Services
{
    public class ReviewService
    {
        private readonly Guid _userId;

        public ReviewService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateReview(ReviewCreate model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var podcast = ctx.Podcasts.Find(model.PodcastId);

                if (podcast == null)
                {
                    return false;
                }

                var review =
                    new Review()
                    {
                        UserId = _userId,
                        Rating = model.Rating,
                        Text = model.Text,
                        PodcastId = model.PodcastId
                    };

                ctx.Reviews.Add(review);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<ReviewListItem> GetReviewsForUser()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Reviews
                        .Where(e => e.UserId == _userId)
                        .Select(
                            e =>
                                new ReviewListItem
                                {
                                    Id = e.Id,
                                    Rating = e.Rating,
                                    Text = e.Text
                                }
                        );
                return query.ToArray();
            }
        }

        public IEnumerable<ReviewListItem> GetReviewsByPodcastId(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Reviews
                        .Where(e => e.PodcastId == id)
                        .Select(
                            e =>
                                new ReviewListItem
                                {
                                    Id = e.Id,
                                    Rating = e.Rating,
                                    Text = e.Text
                                }
                        );
                return query.ToArray();
            }
        }

        public bool UpdateReview(ReviewEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var review =
                    ctx
                        .Reviews
                        .SingleOrDefault(e => e.Id == model.Id && e.UserId == _userId);

                if (review == null)
                {
                    return false;
                }

                review.Rating = model.Rating;
                review.Text = model.Text;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteReview(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var review =
                    ctx
                        .Reviews
                        .SingleOrDefault(e => e.Id == id && e.UserId == _userId);

                if (review == null)
                {
                    return false;
                }

                ctx.Reviews.Remove(review);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
