using PodcastApp.Data;
using PodcastApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastApp.Services
{
    public class PlaylistItemService
    {
        private readonly Guid _userId;

        public PlaylistItemService(Guid userId)
        {
            _userId = userId;
        }

        // POST -- CREATE
        public bool CreatePlaylistItem(PlaylistItemCreate model)
        {
            var entity =
                new PlaylistItem()
                {
                    UserId = _userId,
                    PodcastId = model.PodcastId,
                    EpisodeId = model.EpisodeId,
                    PlaybackPositionInSeconds = 0
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.PlaylistItems.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        // GET -- READ
        public IEnumerable<PlaylistItemDetail> GetPlaylistItems()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .PlaylistItems
                        .Include(e => e.Podcast)
                        .Where(e => e.UserId == _userId)
                        .Select(
                            e =>
                                new PlaylistItemDetail
                                {
                                    UserId = e.UserId,
                                    PodcastId = e.PodcastId,
                                    EpisodeId = e.EpisodeId,
                                    PlaybackPositionInSeconds = e.PlaybackPositionInSeconds
                                }
                        );
                return query.ToArray();
            }
        }

        //GET BY ID ---  READ BY ID
        public PlaylistItemDetail GetPlaylistItemById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .PlaylistItems
                        .Single(e => e.Id == id && e.UserId == _userId);
                return
                    new PlaylistItemDetail
                    {
                        UserId = entity.UserId,
                        PodcastId = entity.PodcastId,
                        EpisodeId = entity.EpisodeId,
                        PlaybackPositionInSeconds = entity.PlaybackPositionInSeconds
                    };
            }
        }

        //UPDATE
        public bool UpdatePlaylistItem(PlaylistItemEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .PlaylistItems
                        .Single(e => e.Id == model.Id && e.UserId == _userId);


                entity.PlaybackPositionInSeconds = model.PlaybackPositionInSeconds;

                return ctx.SaveChanges() == 1;
            }
        }

        // DELETE
        public bool DeletePlaylistItem(int PlaylistItemId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .PlaylistItems
                        .Single(e => e.Id == PlaylistItemId && e.UserId == _userId);

                ctx.PlaylistItems.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
