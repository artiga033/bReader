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
using System.Threading.Tasks;

namespace bReader.Server.Services
{
    public class FeedDataService : IFeedService
    {
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<FeedDbContext> _factory;
        public async Task<IEnumerable<FeedDto>> GetFeedsAsync()
        {
            using var context = _factory.CreateDbContext();
            var entities = await context.Feeds.AsNoTracking().ToListAsync();
            var dtos = _mapper.Map<IEnumerable<FeedDto>>(entities);
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
                    Summary = i.Summary.Substring(0,i.Summary.Length<200?i.Summary.Length:200),
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
            using HttpClient client = new HttpClient();
            Feed entity = await context.Feeds.FirstOrDefaultAsync(x => x.Pk == id);
            if (entity == null) throw new ArgumentException();
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

            await context.SaveChangesAsync();
        }
        public async Task<bool> AddFeedAsync(FeedCreateUpdateDto createDto)
        {
            using var context = _factory.CreateDbContext();
            Feed entity = _mapper.Map<Feed>(createDto);
            context.Feeds.Add(entity);
            return await context.SaveChangesAsync() >= 0;
        }

        public FeedDataService(AutoMapper.IMapper mapper, IDbContextFactory<FeedDbContext> factory)
        {
            this._mapper = mapper;
            this._factory = factory;
        }
    }
}
