using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Linq;

namespace PodcastApp.Data
{
    public class Podcast
    {
        private static readonly XNamespace _itunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";

        public static Podcast CreateFromRssUrl(string rssUrl)
        {
            var rss = XElement.Load(rssUrl);
            var channel = rss.Element("channel");

            var podcast = new Podcast()
            {
                RssUrl = rssUrl,
                XmlCache = rss.ToString(),
                Title = channel.Element("title").Value.Trim(),
                WebsiteUrl = channel.Element("link").Value.Trim(),
                Description = channel.Element(_itunes + "summary").Value.Trim(),
                Author = channel.Element(_itunes + "author").Value.Trim(),
                ImageUrl = channel.Element("image").Element("url").Value.Trim(),
                Category = channel.Element(_itunes + "category").Attribute("text").Value.Trim()
            };

            return podcast;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string RssUrl { get; set; }

        [Required]
        [Column(TypeName ="xml")]
        public string XmlCache { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string WebsiteUrl { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Category { get; set; }

        private List<Episode> _episodes = null;
        public IEnumerable<Episode> Episodes
        {
            get
            {
                if (_episodes == null)
                {
                    _episodes = LoadEpisodes();
                }
                return _episodes;
            }
        }

        public double Rating => Reviews.Any() ? Reviews.Average(review => review.Rating) : 0;

        public virtual List<Review> Reviews { get; set; }
        public virtual List<Subscription> Subscriptions { get; set; }

        private List<Episode> LoadEpisodes()
        {
            var channel = XElement.Parse(XmlCache).Element("channel");
            var episodes = channel.Descendants("item").Select(item => new Episode()
            {
                EpisodeId = item.Element("guid").Value,
                Title = item.Element("title").Value,
                Description = item.Element("description").Value,
                PublishDate = DateTimeOffset.Parse(item.Element("pubDate").Value),
                AudioUrl = item.Element("enclosure").Attribute("url").Value,
                ImageUrl = item.Element(_itunes + "image")?.Attribute("href").Value,
                WebsiteUrl = item.Element("link").Value
            });

            return episodes.ToList();
        }

        public Episode GetEpisode(string episodeId)
        {
            return Episodes.FirstOrDefault(e => e.EpisodeId == episodeId);
        }
    }

    public class Episode
    {
        public string EpisodeId { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }     
        public string AudioUrl { get; set; }
        public string ImageUrl { get; set; }
        public string WebsiteUrl { get; set; }
    }
}
