using PodcastApp.Data;
using PodcastApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public bool HasRssUrl(string rssUrl)
        {
            using (var context = ApplicationDbContext.Create())
            {
                return context.Podcasts.Any(p => p.RssUrl == rssUrl);
            }
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

        public IEnumerable<PodcastListItem> GetAllPodcasts()
        {
            using (var context = ApplicationDbContext.Create())
            {
                var query = context.Podcasts.Select(podcast => new PodcastListItem()
                {
                    Id = podcast.Id,
                    Title = podcast.Title,
                    WebsiteUrl = podcast.WebsiteUrl
                });

                return query.ToArray();
            }
        }

        public PodcastDetail GetPodcastById(int id)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var podcast = context.Podcasts.Find(id);

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
                    WebsiteUrl = podcast.WebsiteUrl,
                    Category = podcast.Category,
                    Rating = podcast.Rating,
                };
            }
        }

        public IEnumerable<EpisodeListItem> GetEpisodesByPodcastId(int id)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var podcast = context.Podcasts.Find(id);

                if (podcast == null)
                {
                    return null;
                }

                var episodes = podcast.Episodes.Select(e => new EpisodeListItem()
                {
                    EpisodeId = e.EpisodeId,
                    PublishDate = e.PublishDate.Date.ToLongDateString(),
                    Title = e.Title,
                    AudioUrl = e.AudioUrl
                });

                return episodes.ToArray();
            }
        }

        public EpisodeDetail GetEpisodeForPodcast(int id, string episodeId)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var podcast = context.Podcasts.Find(id);

                if (podcast == null)
                {
                    return null;
                }

                var episode = podcast.GetEpisode(episodeId);

                if (episode == null)
                {
                    return null;
                }

                return new EpisodeDetail()
                {
                    EpisodeId = episode.EpisodeId,
                    PublishDate = episode.PublishDate.Date.ToLongDateString(),
                    Title = episode.Title,
                    Description = episode.Description,
                    AudioUrl = episode.AudioUrl,
                    ImageUrl = episode.ImageUrl,
                    WebsiteUrl = episode.WebsiteUrl
                };
            }
        }

        public bool DeletePodcast(int id)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var podcast = context.Podcasts.Find(id);

                if (podcast == null)
                {
                    return false;
                }

                context.Podcasts.Remove(podcast);
                return context.SaveChanges() == 1;
            }
        }
    }
}
