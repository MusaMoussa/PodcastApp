using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Models
{
   public class SubscriptionDetail
    {
        public Guid UserId { get; set; }
        public int PodcastId { get; set; }
        public object Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
