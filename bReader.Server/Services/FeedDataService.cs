using AutoMapper;
using bReader.Server.Data;
using bReader.Shared.Models;
using bReader.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;

namespace bReader.Server.Services
{
    public class FeedDataService : IFeedService
    {
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<FeedDbContext> _factory;
        public async Task<ICollection<FeedGroupDto>> GetFeedGroupsAsync()
        {
            using var context = _factory.CreateDbContext();
            var entities = await context.Groups.Include(x => x.Feeds).AsNoTracking().ToListAsync();
            var dtos = _mapper.Map<ICollection<FeedGroupDto>>(entities);
            return dtos;
        }

        public async Task<ICollection<FeedDto>> GetFavoriteFeedsAsync()
        {
            using var context = _factory.CreateDbContext();
            var entities = await context.Feeds.Where(x => x.IsFavorite).ToListAsync();
            var dtos = _mapper.Map<ICollection<FeedDto>>(entities);
            return dtos;
        }

        public async Task<ICollection<FeedItemDto>> GetFavoriteFeedItemsAsync()
        {
            using var context = _factory.CreateDbContext();
            var entities = await context.FeedItems.Where(x => x.IsFavorite).ToListAsync();
            var dtos = _mapper.Map<ICollection<FeedItemDto>>(entities);
            return dtos;
        }

        public async Task<ICollection<FeedItemDto>> GetFeedItemsPreviewAsync(int pk)
        {
            using var context = _factory.CreateDbContext();
            var itemPrev = await context.FeedItems.Where(x => x.SourceFeed.Pk == pk)
               .OrderByDescending(x => DateTimeOffset.Compare(x.LastUpdatedTime, x.PublishDate) < 0 ? x.PublishDate : x.LastUpdatedTime)//because some source dont fill LastUopdateTime field,then we use PublishDate
                .Select(i => new FeedItem
                {
                    Pk = i.Pk,
                    Authors = i.Authors,
                    Categories = i.Categories,
                    IsFavorite = i.IsFavorite,
                    IsRead = i.IsRead,
                    LastUpdatedTime = i.LastUpdatedTime,
                    Links = i.Links,
                    PublishDate = i.PublishDate,
                    SourceFeed = i.SourceFeed,
                    Summary = i.Summary.Substring(0, i.Summary.Length < 200 ? i.Summary.Length : 200),
                    Title = i.Title
                }).AsNoTracking().ToListAsync();
            var dto = _mapper.Map<ICollection<FeedItemDto>>(itemPrev);
            return dto;
        }
        public async Task<FeedItemDto> GetFeedItemAsync(int itemPk)
        {
            using var context = _factory.CreateDbContext();
            var entity = await context.FeedItems.AsNoTracking().FirstOrDefaultAsync(x => x.Pk == itemPk);
            var dto = _mapper.Map<FeedItemDto>(entity);
            return dto;
        }
        public async Task RefreshFeedAsync(int id)
        {
            using var context = _factory.CreateDbContext();
            Feed entity = await context.Feeds.FirstOrDefaultAsync(x => x.Pk == id);
            if (entity == null) throw new ArgumentException();
            await FetchAndUpdateFeedItems(entity, context);
            await context.SaveChangesAsync();
        }
        public Task RefreshAllFeedsAsync()
        {
            throw new NotImplementedException();
        }
        public async Task RefreshAllFeedsAsync(CancellationToken cancellationToken, IProgress<int> progress)
        {
            using var context = _factory.CreateDbContext();
            //var pks = context.Feeds.Select(x => x.Pk);
            int count = 0;
            foreach (var feed in context.Feeds)
            {
                try
                {
                    //TODO : This line got stuck for a long time
                    await FetchAndUpdateFeedItems(feed, context);
                }
                catch
                {/*do nothing, that is, wot't add one on the progress*/}
                progress.Report(++count);
            }
            await context.SaveChangesAsync();
        }
        public Task RefreshAllFeedsAsync(IProgress<int> progress) { throw new NotImplementedException(); }
        public Task RefreshAllFeedsAsync(CancellationToken cancellationToken) { throw new NotImplementedException(); }
        public async Task<bool> AddFeedAsync(FeedCreateUpdateDto createDto, int groupId)
        {
            using var context = _factory.CreateDbContext();
            var group = await context.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group != null)
            {
                var entity = _mapper.Map<Feed>(createDto);
                await FetchAndUpdateFeedItems(entity, context);
                entity.Group = group;
                context.Feeds.Add(entity);
                return await context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> DeleteFeedAsync(int pk)
        {
            using var context = _factory.CreateDbContext();
            var entity = await context.Feeds.FirstOrDefaultAsync(x => x.Pk == pk);
            if (entity != null)
            {
                context.Feeds.Remove(entity);
                return await context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task SetItemsReadAsync(IEnumerable<int> pks)
        {
            using var context = _factory.CreateDbContext();
            await context.Database.ExecuteSqlRawAsync($"UPDATE FeedItems SET IsRead = '1' WHERE Pk IN ({string.Join(',', pks)})");
        }
        public async Task SetItemsNotReadAsync(IEnumerable<int> pks)
        {
            using var context = _factory.CreateDbContext();
            await context.Database.ExecuteSqlRawAsync($"UPDATE FeedItems SET IsRead = '0' WHERE Pk IN ({string.Join(',', pks)})");
        }

        public async Task SetItemsFavoriteAsync(IEnumerable<int> pks)
        {
            using var context = _factory.CreateDbContext();
            await context.Database.ExecuteSqlRawAsync($"UPDATE FeedItems SET IsFavorite = '1' WHERE Pk IN ({string.Join(',', pks)})");
        }
        public async Task SetItemsNotFavoriteAsync(IEnumerable<int> pks)
        {
            using var context = _factory.CreateDbContext();
            await context.Database.ExecuteSqlRawAsync($"UPDATE FeedItems SET IsFavorite = '0' WHERE Pk IN ({string.Join(',', pks)})");
        }

        public async Task SetFeedsFavoriteAsync(IEnumerable<int> pks)
        {
            using var context = _factory.CreateDbContext();
            await context.Database.ExecuteSqlRawAsync($"UPDATE Feeds SET IsFavorite = '1' WHERE Pk IN ({string.Join(',', pks)})");
        }
        public async Task SetFeedsNotFavoriteAsync(IEnumerable<int> pks)
        {
            using var context = _factory.CreateDbContext();
            await context.Database.ExecuteSqlRawAsync($"UPDATE Feeds SET IsFavorite = '0' WHERE Pk IN ({string.Join(',', pks)})");
        }
        public FeedDataService(AutoMapper.IMapper mapper, IDbContextFactory<FeedDbContext> factory)
        {
            this._mapper = mapper;
            this._factory = factory;
        }

        private async Task FetchAndUpdateFeedItems(Feed entity,FeedDbContext context)
        {
            using HttpClient client = new HttpClient();
            var resp = await client.GetAsync(entity.SubscribeLink);
            var synfeed = SyndicationFeed.Load(System.Xml.XmlReader.Create(await resp.Content.ReadAsStreamAsync()));
            var dto = _mapper.Map<FeedDto>(synfeed);
            var newFeed = _mapper.Map<Feed>(dto);
            _mapper.Map(newFeed, entity);

            //the substraction of the new feed and existing, then get new items, comparing rules seperately defined in the EqualityComparer
            var diff = newFeed.Items.Except(entity.Items, new FeedItemSameOldEqualityComparer());
            foreach (var item in diff)
            {
                //set relationship
                item.SourceFeed = entity;

                var existing = await context.FeedItems.SingleOrDefaultAsync(x => x.Id == item.Id);
                if (existing != null)
                {
                    //keep current state
                    item.IsFavorite = existing.IsFavorite;
                    item.IsRead = existing.IsRead;
                    existing = item;
                }
                else
                    context.FeedItems.Add(item);
            }
        }
    }
}
