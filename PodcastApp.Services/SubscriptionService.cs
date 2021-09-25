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
                        Id = e.Id,
                        UserId = e.UserId,
                        PodcastId = e.PodcastId,
                        Title = e.Podcast.Title,
                        AutoAddNewEpisodes = e.AutoAddNewEpisodes
                    });
                return query.ToArray();
            }
        }

        public SubscriptionDetail GetSubscriptionById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var subscription =
                    ctx
                    .Subscriptions
                    .Include(s => s.Podcast)
                    .SingleOrDefault(e => e.Id == id && e.UserId == _userId);

                if (subscription == null)
                {
                    return null;
                }

                return

                    new SubscriptionDetail
                    {
                        UserId = subscription.UserId,
                        PodcastId = subscription.PodcastId,
                        Title = subscription.Podcast.Title,
                        ImageUrl = subscription.Podcast.ImageUrl,
                        AutoAddNewEpisodes = subscription.AutoAddNewEpisodes
                    };

            }
        }

        public bool UpdateSubscription(SubscriptionEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Subscriptions
                    .SingleOrDefault(e => e.Id == model.Id && e.UserId == _userId);

                if (entity == null)
                {
                    return false;
                }

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
                    .SingleOrDefault(e => e.Id == subscriptionId && e.UserId == _userId);

                if (entity == null)
                {
                    return false;
                }

                ctx.Subscriptions.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
