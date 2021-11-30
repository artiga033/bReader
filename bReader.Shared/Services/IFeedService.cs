using bReader.Shared.Models;
using bReader.Shared.Utils;
using System.Linq.Expressions;

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
        public Task<PagedList<FeedItemDto>> GetFavoriteFeedItemsAsync(int page);
        public Task<PagedList<FeedItemDto>> GetUnreadFeedItemsAsync(int page);
        public Task<PagedList<FeedItemDto>> GetFeedItemsPreviewAsync(int feedPk, int page);
        public Task<PagedList<FeedItemDto>> GetFeedItemsPreviewAsync(int page);
        /// <summary>
        /// use an queryExpression to query data.
        /// Add specific method to the service class instead of directly using this method.
        /// Will be private in the future
        /// </summary>
        /// <param name="query">the query expression, which will be used in Where clause</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Obsolete("Avoid using this method")]
        public Task<PagedList<FeedItemDto>> GetFeedItemsPreviewAsync(Expression<Func<FeedItem, bool>> query, int page);
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

        public Task<bool> AddFeedAsync(FeedCreateUpdateDto createDto, int groupId);
        public Task<bool> DeleteFeedAsync(int pk);

        public Task SetItemsReadAsync(IEnumerable<int> pks);
        public Task SetItemsNotReadAsync(IEnumerable<int> pks);

        public Task SetItemsFavoriteAsync(IEnumerable<int> pks);
        public Task SetItemsNotFavoriteAsync(IEnumerable<int> pks);

        public Task SetFeedsFavoriteAsync(IEnumerable<int> pks);
        public Task SetFeedsNotFavoriteAsync(IEnumerable<int> pks);

    }
}
