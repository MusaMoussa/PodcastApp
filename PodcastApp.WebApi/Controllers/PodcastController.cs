using Microsoft.AspNet.Identity;
using PodcastApp.Models;
using PodcastApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PodcastApp.WebApi.Controllers
{
    [Authorize]
    public class PodcastController : ApiController
    {
        private PodcastService CreatePodcastService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            return new PodcastService(userId);
        }

        // GET: api/Podcast
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Podcast/5
        public IHttpActionResult Get(int id)
        {
            var service = CreatePodcastService();
            var podcast = service.GetPodcastById(id);

            if (podcast == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(podcast);
            }
        }

        // POST: api/Podcast
        public IHttpActionResult Post([FromBody] PodcastCreate model)
        {
            if (model == null)
            {
                return BadRequest("Http Request Body cannot be empty!");
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var service = CreatePodcastService();

            if (service.CreatePodcast(model))
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
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
