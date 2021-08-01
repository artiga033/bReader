using bReader.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared.Services
{
    public interface IFeedService
    {
        /// <summary>
        /// Update feed from the feed's SubscribeLink, and save the result.
        /// </summary>
        /// <param name="feed"></param>
        public Task RefreshFeedAsync(int id);
        /// <summary>
        /// Get the feeds from storage.
        /// </summary>
        public Task<IEnumerable<FeedDto>> GetFeedsAsync();
        /// <summary>
        /// Get the specific feed, with its items.
        /// </summary>
        /// <returns></returns>
        public Task<ICollection<FeedItemDto>> GetFeedItemsPreviewAsync(int pk);
        public Task<FeedItemDto> GetFeedItemAsync(int Pk);

        public Task<bool> AddFeedAsync(FeedCreateUpdateDto createDto);
    }
}
