using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using bReader.Shared.Models;
using bReader.Shared.Services;
using Blazored.LocalStorage;
using AutoMapper;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Linq;
using bReader.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace bReader.Wasm.Services
{
    public class FeedDataService : IFeedService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        const string KEY_GROUPS = "feed_groups";
        /// <summary>
        /// This Service is added with AddScoped, meaning that every time a new browser session 
        /// was opened, there would be a new service istance created, so we just save all refs of items here
        /// for quicker query.
        /// </summary>
        private List<FeedItemDto> items = new();
        public async Task<ICollection<FeedGroupDto>> GetFeedGroupsAsync()
        {
            var entities = await _localStorage.GetItemAsync<ICollection<FeedGroup>>(KEY_GROUPS);
            if (entities == null)
            {
                //first run, initialize a default group
                var defaultGroup = new List<FeedGroup>() { new FeedGroup { Id = 0, Name = "默认", Feeds = new List<Feed>() } };
                await _localStorage.SetItemAsync(KEY_GROUPS, defaultGroup);
                return _mapper.Map<ICollection<FeedGroupDto>>(defaultGroup);
            }
            else
                return _mapper.Map<ICollection<FeedGroupDto>>(entities);
        }

        public Task<ICollection<FeedDto>> GetFavoriteFeedsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<FeedItemDto>> GetFavoriteFeedItemsAsync()
        {
            return null;
        }

        public async Task<ICollection<FeedItemDto>> GetFeedItemsPreviewAsync(int pk)
        {
            var feeds = Extensions.GetAllFeeds(await GetFeedGroupsAsync());
            var target = feeds.FirstOrDefault(x => x.Pk == pk);
            await FetchAndUpdateFeedItems(target);
            return target.Items;
        }

        public async Task<FeedItemDto> GetFeedItemAsync(int pk)
        {
            var feeds = Extensions.GetAllFeeds(await GetFeedGroupsAsync());
            var target = this.items.FirstOrDefault(x => x.Pk == pk);
            return target;
        }

        public Task RefreshFeedAsync(int id)
        {
            return Task.CompletedTask;
        }

        public Task RefreshAllFeedsAsync()
        {
            return Task.CompletedTask;
        }

        public Task RefreshAllFeedsAsync(CancellationToken cancellationToken, IProgress<int> progress)
        {
            return Task.CompletedTask;
        }

        public Task RefreshAllFeedsAsync(IProgress<int> progress)
        {
            return Task.CompletedTask;
        }

        public Task RefreshAllFeedsAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<bool> AddFeedAsync(FeedCreateUpdateDto createDto, int groupId)
        {
            var entities = await _localStorage.GetItemAsync<ICollection<FeedGroup>>(KEY_GROUPS);
            var feedToAdd = _mapper.Map<Feed>(createDto);
            var targetGroup = entities.FirstOrDefault(x => x.Id == groupId);
            if (targetGroup != null)
            {
                var reqMsg = new HttpRequestMessage(HttpMethod.Get, feedToAdd.SubscribeLink);
                //the default implement of HttpClient in Blazor Wasm is using fetch API(https://developer.mozilla.org/zh-CN/docs/Web/API/Fetch_API/Using_Fetch)
                //in this case, it would cause many trouble
                reqMsg.SetBrowserRequestMode(BrowserRequestMode.NoCors);
                var resp = await _httpClient.SendAsync(reqMsg);
                var synfeed = SyndicationFeed.Load(System.Xml.XmlReader.Create(await resp.Content.ReadAsStreamAsync()));
                var dto = _mapper.Map<FeedDto>(synfeed);
                _mapper.Map(dto, feedToAdd);

                targetGroup.Feeds.Add(feedToAdd);
                await _localStorage.SetItemAsync<ICollection<FeedGroup>>(KEY_GROUPS, entities);
            }
            return false;
        }

        public async Task<bool> DeleteFeedAsync(int pk)
        {
            var entities = await _localStorage.GetItemAsync<ICollection<FeedGroup>>(KEY_GROUPS);
            var feeds = entities.GetAllFeeds();
            var todelete = feeds.FirstOrDefault(x => x.Pk == pk);
            if (todelete != null)
            {
                feeds.Remove(todelete);
                await _localStorage.SetItemAsync<ICollection<FeedGroup>>(KEY_GROUPS, entities);
            }
            return false;
        }

        public Task SetItemsReadAsync(IEnumerable<int> pks)
        {
            return Task.CompletedTask;
        }

        public Task SetItemsNotReadAsync(IEnumerable<int> pks)
        {
            return Task.CompletedTask;
        }

        public Task SetItemsFavoriteAsync(IEnumerable<int> pks)
        {
            return Task.CompletedTask;
        }

        public Task SetItemsNotFavoriteAsync(IEnumerable<int> pks)
        {
            return Task.CompletedTask;
        }

        public async Task SetFeedsFavoriteAsync(IEnumerable<int> pks)
        {
            var entities = await _localStorage.GetItemAsync<ICollection<FeedGroup>>(KEY_GROUPS);
            var feeds = entities.GetAllFeeds();
            foreach (var feed in feeds)
            {
                if (pks.Contains(feed.Pk))
                    feed.IsFavorite = true;
            }
            await _localStorage.SetItemAsync<ICollection<FeedGroup>>(KEY_GROUPS, entities);
        }

        public async Task SetFeedsNotFavoriteAsync(IEnumerable<int> pks)
        {
            var entities = await _localStorage.GetItemAsync<ICollection<FeedGroup>>(KEY_GROUPS);
            var feeds = entities.GetAllFeeds();
            foreach (var feed in feeds)
            {
                if (pks.Contains(feed.Pk))
                    feed.IsFavorite = false;
            }
            await _localStorage.SetItemAsync<ICollection<FeedGroup>>(KEY_GROUPS, entities);
        }

        /// <summary>
        /// Receives a FeedDto and update it's Items Property.
        /// As in this WebAssembly version, we don't save items data.
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        private async Task FetchAndUpdateFeedItems(FeedDto feed)
        {
            var resp = await _httpClient.GetAsync(feed.SubscribeLink);
            var synfeed = SyndicationFeed.Load(System.Xml.XmlReader.Create(await resp.Content.ReadAsStreamAsync()));
            var dto = _mapper.Map<FeedDto>(synfeed);
            _mapper.Map(dto, feed);

            foreach (var item in dto.Items)
            {
                //set relationship
                item.SourceFeed = feed;

                var existing = feed.Items.FirstOrDefault(x => x.Id == item.Id);
                if (existing != null)
                {
                    //keep current state
                    item.IsFavorite = existing.IsFavorite;
                    item.IsRead = existing.IsRead;
                    item.Pk = existing.Pk;
                    existing = item;
                }
                else
                {
                    item.Pk = items.Count;
                    feed.Items.Add(item);
                    this.items.Add(item);
                }
            }
        }
        public FeedDataService(ILocalStorageService localStorage, AutoMapper.IMapper mapper, HttpClient httpClient)
        {
            this._localStorage = localStorage;
            this._mapper = mapper;
            this._httpClient = httpClient;
        }

    }
}
