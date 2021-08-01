using Blazor.Extensions.Storage;
using bReader.Shared.Models;
using bReader.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bReader.Wasm.Services
{
    public class FeedDataService : IFeedService
    {
        private readonly LocalStorage _localStorage;

        const string KEY_LINKS = "subscribeLinks";

        public Task RefreshFeedAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FeedDto>> GetFeedsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<FeedItemDto>> GetFeedItemsPreviewAsync(int pk)
        {
            throw new NotImplementedException();
        }

        public Task<FeedItemDto> GetFeedItemAsync(int itemPk)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddFeedAsync(FeedCreateUpdateDto createDto)
        {
            throw new NotImplementedException();
        }

        public FeedDataService(LocalStorage localStorage)
        {
            this._localStorage = localStorage;
        }
    }
}
