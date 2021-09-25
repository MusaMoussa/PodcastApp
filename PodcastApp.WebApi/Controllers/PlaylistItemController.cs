using Microsoft.AspNet.Identity;
using PodcastApp.Models;
using PodcastApp.Services;
using System;
using System.Web.Http;

namespace PodcastApp.WebApi.Controllers
{
    public class PlaylistItemController : ApiController
    {
        private PlaylistItemService CreatePlaylistItemService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var playlistItemService = new PlaylistItemService(userId);
            return playlistItemService;
        }

        // POST -- CREATE
        public IHttpActionResult Post(PlaylistItemCreate playlistItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreatePlaylistItemService();

            if (!service.CreatePlaylistItem(playlistItem))
                return InternalServerError();
            return Ok();
        }

        // GET All -- READ ALL
        public IHttpActionResult Get()
        {
            PlaylistItemService playlistItemService = CreatePlaylistItemService();
            var playlistItems = playlistItemService.GetPlaylistItems();
            return Ok(playlistItems);
        }

        // GET by id  -- READ by id
        public IHttpActionResult Get(int id)
        {
            PlaylistItemService playlistItemService = CreatePlaylistItemService();
            var playlistItem = playlistItemService.GetPlaylistItemById(id);

            if (playlistItem == null)
            {
                return NotFound();
            }
            return Ok(playlistItem);
        }

        // PUT -- UPDATE
        public IHttpActionResult Put(PlaylistItemEdit playlistItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreatePlaylistItemService();

            if (!service.UpdatePlaylistItem(playlistItem))
                return InternalServerError();

            return Ok();
        }

        // DELETE
        public IHttpActionResult Delete(int id)
        {
            var service = CreatePlaylistItemService();

            if (!service.DeletePlaylistItem(id))
                return InternalServerError();

            return Ok();
        }
    }
}
