using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Data
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(Podcast))]
        public int PodcastId { get; set; }
        public virtual Podcast Podcast { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
