using PodcastApp.Data;
using PodcastApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml.Linq;

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
                var query = context.Podcasts.Select(podcast => new PodcastListItem
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

                var episodes = podcast.Episodes.Select(e => new EpisodeListItem
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

                return new EpisodeDetail
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

        public bool UpdatePodcast(int id)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var podcast = context.Podcasts.Include(p => p.Subscriptions).SingleOrDefault(p => p.Id == id);

                if (podcast == null)
                {
                    return false;
                }

                // Find latest episode ids
                var updatedRss = XElement.Load(podcast.RssUrl);
                string latestCachedEpisodeId = XElement.Parse(podcast.XmlCache).Element("channel").Element("item").Element("guid").Value;
                List<string> latestEpisodeIds = GetLatestEpisodeIds(updatedRss, latestCachedEpisodeId);

                // If we found new episode ids
                if (latestEpisodeIds.Count > 0)
                {
                    // Update Podcast Properties
                    podcast.XmlCache = updatedRss.ToString();
                    podcast.ClearEpisodes();

                    // Add latest episodes to every subscriber's playlist
                    foreach (var subscription in podcast.Subscriptions)
                    {
                        if (subscription.AutoAddNewEpisodes)
                        {
                            foreach (string episodeId in latestEpisodeIds)
                            {
                                var playlistItem = new PlaylistItem
                                {
                                    UserId = subscription.UserId,
                                    PodcastId = subscription.PodcastId,
                                    EpisodeId = episodeId
                                };
                                context.PlaylistItems.Add(playlistItem);
                            }
                        }
                    }

                    context.SaveChanges();
                }

                return true;
            }
        }

        private List<string> GetLatestEpisodeIds(XElement updatedRss, string lastKnownEpisodeId)
        {
            var output = new List<string>();
            var items = updatedRss.Element("channel").Descendants("item");

            foreach (var item in items)
            {
                string id = item.Element("guid").Value;
                if (id == lastKnownEpisodeId)
                {
                    break;
                }
                output.Add(id);
            }

            return output;
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
