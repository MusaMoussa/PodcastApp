using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PodcastApp.Data
{
    public class PlaylistItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(Podcast))]
        public int PodcastId { get; set; }
        public virtual Podcast Podcast { get; set; }

        [Required]
        public string EpisodeId { get; set; }

        [Required]
        public int PlaybackPositionInSeconds { get; set; }
    }
}
