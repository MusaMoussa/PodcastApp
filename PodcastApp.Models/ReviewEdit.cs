using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
