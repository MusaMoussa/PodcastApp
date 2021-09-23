using PodcastApp.Data;
using PodcastApp.Models;
using System;
using System.Collections.Generic;
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
					PlaybackPositionInSeconds = model.PlaybackPositionInSeconds
				};

			using (var ctx = new ApplicationDbContext())
			{
				ctx.PlaylistItems.Add(entity);
				return ctx.SaveChanges() == 1;
			}
		}

		// GET -- READ
		public IEnumerable<PlaylistItem> GetPlaylistItems()
		{
			using (var ctx = new ApplicationDbContext())
			{
				var query =
					ctx
						.PlaylistItems
						.Where(e => e.UserId == _userId)
						.Select(
							e =>
								new PlaylistItem
								{
									Id = e.Id,
									PodcastId = e.PodcastId,
									EpisodeId = e.EpisodeId
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
						UserId = _userId,
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

				entity.PodcastId = model.PodcastId;
				entity.EpisodeId = model.EpisodeId;
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
