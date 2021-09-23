using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Models
{
    public class ReviewDetail
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(Podcast))]
        public int PodcastId { get; set; }
        public virtual Podcast Podcast { get; set; }

        [Required]
        [Range(0, 5)]
        public double Rating { get; set; }

        [Required]
        public string Text { get; set; }

        [Display(Name = "Posted")]
        public DateTimeOffset CreatedUtc { get; set; }
    }
}

