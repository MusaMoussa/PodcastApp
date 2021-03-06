using System;

namespace PodcastApp.Models
{
    public class ReviewDetail
    {

        public int Id { get; set; }

        public Guid UserId { get; set; }

        public int PodcastId { get; set; }
        
        public string PodcastTitle { get; set; }

        public double Rating { get; set; }

        public string Text { get; set; }

        public DateTimeOffset CreatedUtc { get; set; }
    }
}

