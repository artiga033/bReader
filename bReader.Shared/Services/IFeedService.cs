using bReader.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bReader.Shared.Services
{
    public interface IFeedService
    {
 
        /// <summary>
        /// Get grouped feeds from storage.
        /// </summary>
        public Task<ICollection<FeedGroupDto>> GetFeedGroupsAsync();
        [Obsolete("You may use GetFeedAsync() and query the favorite ones yourself")]
        public Task<ICollection<FeedDto>> GetFavoriteFeedsAsync();
        public Task<ICollection<FeedItemDto>> GetFavoriteFeedItemsAsync();
        public Task<ICollection<FeedItemDto>> GetFeedItemsPreviewAsync(int pk);
        public Task<FeedItemDto> GetFeedItemAsync(int pk);

        /// <summary>
        /// Update feed from the feed's SubscribeLink, and save the result.
        /// </summary>
        /// <param name="feed"></param>
        public Task RefreshFeedAsync(int id);
        public Task RefreshAllFeedsAsync();
        public Task RefreshAllFeedsAsync(CancellationToken cancellationToken, IProgress<int> progress);
        public Task RefreshAllFeedsAsync(IProgress<int> progress);
        public Task RefreshAllFeedsAsync(CancellationToken cancellationToken);

        public Task<bool> AddFeedAsync(FeedCreateUpdateDto createDto,int groupId);
        public Task<bool> DeleteFeedAsync(int pk);

        public Task SetItemsReadAsync(IEnumerable<int> pks);
        public Task SetItemsNotReadAsync(IEnumerable<int> pks);

        public Task SetItemsFavoriteAsync(IEnumerable<int> pks);
        public Task SetItemsNotFavoriteAsync(IEnumerable<int> pks);

        public Task SetFeedsFavoriteAsync(IEnumerable<int> pks);
        public Task SetFeedsNotFavoriteAsync(IEnumerable<int> pks);

    }
}
