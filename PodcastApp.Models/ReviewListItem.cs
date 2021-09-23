using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Models
{
    public class ReviewListItem
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public string Text { get; set; }

    }
}
