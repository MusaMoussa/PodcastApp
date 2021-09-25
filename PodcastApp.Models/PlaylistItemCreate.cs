using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
