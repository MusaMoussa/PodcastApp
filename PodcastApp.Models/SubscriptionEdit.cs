using System.ComponentModel.DataAnnotations;

namespace PodcastApp.Models
{
    public class SubscriptionEdit
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool AutoAddNewEpisodes { get; set; }
    }
}
