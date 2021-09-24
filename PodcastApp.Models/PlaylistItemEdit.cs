using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
