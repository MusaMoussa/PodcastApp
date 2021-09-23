using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Models
{
	public class PlaylistItemDetail
	{
        public Guid UserId { get; set; }

        public int PodcastId { get; set; }

        public string EpisodeId { get; set; }

        public int PlaybackPositionInSeconds { get; set; }
    }
}
