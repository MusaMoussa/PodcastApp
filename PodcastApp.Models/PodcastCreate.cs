using System.ComponentModel.DataAnnotations;

namespace PodcastApp.Models
{
    public class PodcastCreate
    {
        [Required]
        public string RssUrl { get; set; }
    }
}
