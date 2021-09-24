using PodcastApp.Data;
using PodcastApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Services
{
    public class PodcastService
    {
        private readonly Guid _userId;

        public PodcastService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreatePodcast(PodcastCreate model)
        {
            var podcast = Podcast.CreateFromRssUrl(model.RssUrl);

            using (var context = ApplicationDbContext.Create())
            {
                context.Podcasts.Add(podcast);
                return context.SaveChanges() == 1;
            }
        }

        public PodcastDetail GetPodcastById(int id)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var podcast = context.Podcasts.FirstOrDefault(p => p.Id == id);

                if (podcast == null)
                {
                    return null;
                }

                return new PodcastDetail
                {
                    Id = podcast.Id,
                    RssUrl = podcast.RssUrl,
                    Title = podcast.Title,
                    Description = podcast.Description,
                    Author = podcast.Author,
                    ImageUrl = podcast.ImageUrl,
                    Category = podcast.Category,
                    Rating = podcast.Rating,
                };
            }
        }
    }
}
