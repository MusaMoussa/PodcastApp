using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Models
{
	public class PlaylistItemDetail
	{
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int PodcastId { get; set; }
        public string PodcastTitle { get; set; }
        public string EpisodeId { get; set; }
        public string EpisodeTitle { get; set; }
        public string AudioUrl { get; set; }
        public int PlaybackPositionInSeconds { get; set; }
    }
}
