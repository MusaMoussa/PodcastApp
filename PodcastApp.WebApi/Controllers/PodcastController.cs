using Microsoft.AspNet.Identity;
using PodcastApp.Models;
using PodcastApp.Services;
using System;
using System.Net;
using System.Web.Http;
using System.Xml;

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
        public IHttpActionResult Get()
        {
            var service = CreatePodcastService();
            var podcasts = service.GetAllPodcasts();
            return Ok(podcasts);
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

        // GET: api/Podcast/{id}/Episode
        [HttpGet]
        [Route("api/Podcast/{id}/Episode")]
        public IHttpActionResult GetAllEpisodesForPodcast(int id)
        {
            var service = CreatePodcastService();
            var episodes = service.GetEpisodesByPodcastId(id);
            return Ok(episodes);
        }

        // GET: api/Podcast/{id}/Episode?episodeId={episodeId}
        [HttpGet]
        [Route("api/Podcast/{id}/Episode")]
        public IHttpActionResult GetEpisodeForPodcast(int id, string episodeId)
        {
            var service = CreatePodcastService();
            var episode = service.GetEpisodeForPodcast(id, episodeId);

            if (episode == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(episode);
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
            
            if (service.HasRssUrl(model.RssUrl))
            {
                return BadRequest("Podcast is already in the database!");
            }

            try
            {
                if (service.CreatePodcast(model))
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (WebException exception)
            {
                return BadRequest($"Using RssUrl: {model.RssUrl}, {exception.Message}");
            }
            catch (XmlException)
            {
                return BadRequest("Xml Read Error");
            }
        }

        // PUT: api/Podcast/5
        public IHttpActionResult Put(int id)
        {
            var service = CreatePodcastService();

            if (service.UpdatePodcast(id))
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
        }

        // DELETE: api/Podcast/5
        public IHttpActionResult Delete(int id)
        {
            var service = CreatePodcastService();
            
            if (service.DeletePodcast(id))
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
        }
    }
}
