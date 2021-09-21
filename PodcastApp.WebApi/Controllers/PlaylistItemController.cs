using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PodcastApp.WebApi.Controllers
{
    public class PlaylistItemController : ApiController
    {
        // GET: api/PlaylistItem
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/PlaylistItem/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PlaylistItem
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PlaylistItem/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PlaylistItem/5
        public void Delete(int id)
        {
        }
    }
}
