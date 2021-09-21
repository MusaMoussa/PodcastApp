using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PodcastApp.WebApi.Controllers
{
    public class PodcastController : ApiController
    {
        // GET: api/Podcast
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Podcast/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Podcast
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Podcast/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Podcast/5
        public void Delete(int id)
        {
        }
    }
}
