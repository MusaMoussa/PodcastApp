using System.ComponentModel.DataAnnotations;

namespace PodcastApp.Models
{
    public class ReviewEdit
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }
    }
}
