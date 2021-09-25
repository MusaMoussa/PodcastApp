using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Models
{
    public class EpisodeDetail
    {
        public string EpisodeId { get; set; }
        public string PublishDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AudioUrl { get; set; }
        public string ImageUrl { get; set; }
        public string WebsiteUrl { get; set; }
    }
}
