using System;

namespace PodcastApp.Models
{
    public class SubscriptionDetail
    {
        public Guid UserId { get; set; }
        public int PodcastId { get; set; }
        public object Title { get; set; }
        public string ImageUrl { get; set; }
        public object AutoAddNewEpisodes { get; set; }
    }
}
