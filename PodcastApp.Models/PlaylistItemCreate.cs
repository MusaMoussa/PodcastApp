using System.ComponentModel.DataAnnotations;

namespace PodcastApp.Models
{
    public class PlaylistItemCreate
	{
        [Required]
        public int PodcastId { get; set; }

        [Required]
        public string EpisodeId { get; set; }
    }
}
