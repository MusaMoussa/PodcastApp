using System.ComponentModel.DataAnnotations;

namespace PodcastApp.Models
{
    public class PlaylistItemEdit
	{
        [Required]
		public int Id { get; set; }

        [Required]
        public int PlaybackPositionInSeconds { get; set; }
    }
}
