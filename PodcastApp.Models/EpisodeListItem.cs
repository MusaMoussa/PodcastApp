using System;

namespace PodcastApp.Models
{
    public class EpisodeListItem
    {
        public string EpisodeId { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public string Title { get; set; }
        public string AudioUrl { get; set; }
    }
}
