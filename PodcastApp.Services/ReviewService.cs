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
            var query =
                new Review()
                {
                    UserId = _userId,
                    Rating = model.Rating,
                    Text = model.Text,
                    PodcastId = model.PodcastId
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Reviews.Add(query);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<ReviewListItem> GetReviews()
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
                var query =
                    ctx
                        .Reviews
                        .Single(e => e.Id == model.Id && e.UserId == _userId);

                query.Rating = model.Rating;
                query.Text = model.Text;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteReview(int id)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Reviews
                        .Single(e => e.Id == id && e.UserId == _userId);

                ctx.Reviews.Remove(query);

                return ctx.SaveChanges() == 1;
            }
        }
    }

   



}
