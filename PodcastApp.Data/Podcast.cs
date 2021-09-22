using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PodcastApp.Data
{
    public class Podcast
    {
        public static Podcast CreateFromRssUrl(string rssUrl)
        {
            string itunesSchema = "http://www.itunes.com/dtds/podcast-1.0.dtd";
            //XElement xml = XElement.Load(rssUrl);
            XmlReader reader = XmlReader.Create(rssUrl);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            var podcast = new Podcast()
            {
                RssUrl = rssUrl,
                Title = feed.Title.Text,
                Description = feed.Description.Text,
                ImageUrl = feed.ImageUrl.ToString(),
                Category = feed.ElementExtensions.ReadElementExtensions<XmlElement>("category", itunesSchema)[0].GetAttribute("text")
            };

            // Category is <itunes:category text="" />
            Console.WriteLine(feed.ElementExtensions.ReadElementExtensions<XmlElement>("category", itunesSchema)[0].GetAttribute("text"));

            return podcast;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string RssUrl { get; set; }

        [Required]
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

        // public virtual List<Review> Reviews { get; set; }
        // public virtual List<Subscription> Subscriptions { get; set; }

        private List<Episode> LoadEpisodes()
        {
            throw new NotImplementedException();
        }
    }

    public class Episode
    {
        public int Id { get; set; }
        public Podcast Podcast { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public string AudioUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}
