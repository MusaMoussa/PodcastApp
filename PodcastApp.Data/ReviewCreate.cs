using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Data
{
    public class ReviewCreate
    {
        public virtual Podcast Podcast { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }

        [Required]
        public double Rating { get; set; }
    }
}
