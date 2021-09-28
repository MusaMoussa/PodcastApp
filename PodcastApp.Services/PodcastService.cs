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
        private static readonly XNamespace _itunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";
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
            var podcast = CreateFromRssUrl(model.RssUrl);

            using (var context = ApplicationDbContext.Create())
            {
                context.Podcasts.Add(podcast);
                return context.SaveChanges() == 1;
            }
        }

        private Podcast CreateFromRssUrl(string rssUrl)
        {
            var podcast = new Podcast { RssUrl = rssUrl };
            var rss = XElement.Load(rssUrl);
            UpdatePodcastProperties(podcast, rss);
            return podcast;
        }

        private void UpdatePodcastProperties(Podcast podcast, XElement rss)
        {
            podcast.XmlCache = rss.ToString();
            var channel = rss.Element("channel");
            podcast.Title = channel.Element("title").Value.Trim();
            podcast.WebsiteUrl = channel.Element("link").Value.Trim();
            podcast.Description = channel.Element(_itunes + "summary").Value.Trim();
            podcast.Author = channel.Element(_itunes + "author").Value.Trim();
            podcast.ImageUrl = channel.Element("image").Element("url").Value.Trim();
            podcast.Category = channel.Element(_itunes + "category").Attribute("text").Value.Trim();
            podcast.ClearEpisodes();
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
                var podcast = context.Podcasts
                    .Include(p => p.Subscriptions)
                    .SingleOrDefault(p => p.Id == id);

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
                    UpdatePodcastProperties(podcast, updatedRss);
                    context.SaveChanges();

                    // Add latest episodes to every subscriber's playlist
                    List<PlaylistItem> items = CreatePlaylistItemsForSubscriptions(podcast.Subscriptions, latestEpisodeIds);
                    context.PlaylistItems.AddRange(items);
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

        private List<PlaylistItem> CreatePlaylistItemsForSubscriptions(IEnumerable<Subscription> subscriptions, IEnumerable<string> latestEpisodeIds)
        {
            var output = new List<PlaylistItem>();

            foreach (var subscription in subscriptions)
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
                        output.Add(playlistItem);
                    }
                }
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
