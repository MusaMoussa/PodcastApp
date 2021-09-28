using System.ComponentModel.DataAnnotations;

namespace PodcastApp.Data
{
    public class ReviewCreate
    {
        [Required]
        public int PodcastId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }

        [Required]
        public double Rating { get; set; }
    }
}
