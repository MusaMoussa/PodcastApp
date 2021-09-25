using System.ComponentModel.DataAnnotations;

namespace PodcastApp.Models
{
    public class SubscriptionCreate
    {
        [Required]
        public int PodcastId { get; set; }
    }
}
