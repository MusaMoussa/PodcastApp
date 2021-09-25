using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Models
{
    public class SubscriptionListItem
    {
        public Guid UserId { get; set; }
        public int PodcastId { get; set; }
        public string Title { get; set; }
    }
}
