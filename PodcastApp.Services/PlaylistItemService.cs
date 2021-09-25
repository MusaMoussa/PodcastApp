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
                        .Where(e => e.UserId == _userId);

                var output = new List<PlaylistItemDetail>();

                foreach (var item in query)
                {
                    var episode = item.Podcast.GetEpisode(item.EpisodeId);
                    var detail = new PlaylistItemDetail
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        PodcastId = item.PodcastId,
                        PodcastTitle = item.Podcast.Title,
                        EpisodeId = item.EpisodeId,
                        EpisodeTitle = episode.Title,
                        AudioUrl = episode.AudioUrl,
                        PlaybackPositionInSeconds = item.PlaybackPositionInSeconds
                    };
                    output.Add(detail);
                }

                return output;
            }
        }

        //GET BY ID ---  READ BY ID
        public PlaylistItemDetail GetPlaylistItemById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var item =
                    ctx
                        .PlaylistItems
                        .SingleOrDefault(e => e.Id == id && e.UserId == _userId);

                if (item == null)
                {
                    return null;
                }

                var episode = item.Podcast.GetEpisode(item.EpisodeId);

                return
                    new PlaylistItemDetail
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        PodcastId = item.PodcastId,
                        PodcastTitle = item.Podcast.Title,
                        EpisodeId = item.EpisodeId,
                        EpisodeTitle = episode.Title,
                        AudioUrl = episode.AudioUrl,
                        PlaybackPositionInSeconds = item.PlaybackPositionInSeconds
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
                        .SingleOrDefault(e => e.Id == model.Id && e.UserId == _userId);

                if (entity == null)
                {
                    return false;
                }

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
                        .SingleOrDefault(e => e.Id == PlaylistItemId && e.UserId == _userId);

                if (entity == null)
                {
                    return false;
                }

                ctx.PlaylistItems.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
