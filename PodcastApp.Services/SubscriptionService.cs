using PodcastApp.Data;
using PodcastApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PodcastApp.Services
{
   public class SubscriptionService
    {
        private readonly Guid _userId;

        public SubscriptionService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateSubscription(SubscriptionCreate model)
        {
            var entity =
                new Subscription()
                {
                    UserId = _userId,
                    PodcastId = model.PodcastId,
                    AutoAddNewEpisodes = true
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Subscriptions.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<SubscriptionListItem> GetSubscriptions()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Subscriptions
                    .Include(s => s.Podcast)
                    .Where(e => e.UserId == _userId)
                    .Select(e =>
                    new SubscriptionListItem
                    {
                        UserId = e.UserId,
                        PodcastId = e.PodcastId,
                        Title = e.Podcast.Title
                    });
                return query.ToArray();
            }
        }

        public SubscriptionDetail GetSubscriptionById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Subscriptions
                    .Include(s => s.Podcast)
                    .Single(e => e.Id == id && e.UserId == _userId);
                  
                return

                    new SubscriptionDetail
                    {
                        UserId = entity.UserId,
                        PodcastId = entity.PodcastId,
                        Title = entity.Podcast.Title,
                        ImageUrl = entity.Podcast.ImageUrl
                    };
                
            }
        }

        public bool UpdateSubscription(SubscriptionEdit model)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Subscriptions
                    .Single(e => e.Id == model.Id && e.UserId == _userId);

                entity.AutoAddNewEpisodes = model.AutoAddNewEpisodes;

                return ctx.SaveChanges() == 1;

            }
        }

        public bool DeleteSubscription(int subscriptionId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Subscriptions
                    .Single(e => e.Id == subscriptionId && e.UserId == _userId);
                ctx.Subscriptions.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
