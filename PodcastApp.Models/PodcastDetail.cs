using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Models
{
    public class PodcastDetail
    {
        public int Id { get; set; }
        public string RssUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public double Rating { get; set; }
        public int EpisodeCount { get; set; }
        public string XmlCache { get; set; }
    }
}
