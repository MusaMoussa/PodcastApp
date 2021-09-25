using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Models
{
    public class SubscriptionEdit
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public bool AutoAddNewEpisodes { get; set; }
    }
}
