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
        /// <summary>
        /// Add a new item to a user's playlist
        /// </summary>
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
        /// <summary>
        /// Get all the item's from a user's playlist
        /// </summary>
        /// <returns>PlaylistItemDetail</returns>
        public IHttpActionResult Get()
        {
            PlaylistItemService playlistItemService = CreatePlaylistItemService();
            var playlistItems = playlistItemService.GetPlaylistItems();
            return Ok(playlistItems);
        }

        // GET by id  -- READ by id
        /// <summary>
        /// Get detailed information for a playlist item
        /// </summary>
        /// <returns>PlaylistItemDetail</returns>
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
        /// <summary>
        /// Updates information for a playlist item
        /// </summary>
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
        /// <summary>
        /// Deletes a playlist item
        /// </summary>
        public IHttpActionResult Delete(int id)
        {
            var service = CreatePlaylistItemService();

            if (!service.DeletePlaylistItem(id))
                return InternalServerError();

            return Ok();
        }
    }
}
