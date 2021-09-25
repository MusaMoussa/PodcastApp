namespace PodcastApp.Models
{
    public class PodcastDetail
    {
        public int Id { get; set; }
        public string RssUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string WebsiteUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public double Rating { get; set; }    
    }
}
