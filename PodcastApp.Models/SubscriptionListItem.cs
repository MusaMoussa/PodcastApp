using System;

namespace PodcastApp.Models
{
    public class SubscriptionListItem
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int PodcastId { get; set; }
        public string Title { get; set; }
        public bool AutoAddNewEpisodes { get; set; }
    }
}
