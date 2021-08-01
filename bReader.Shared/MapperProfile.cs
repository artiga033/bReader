using AutoMapper;
using bReader.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.Json;
using System.Threading.Tasks;

namespace bReader.Shared
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<string, ICollection<PersonDto>>().ConvertUsing(typeof(JsonDeserializeConverter<ICollection<PersonDto>>));
            CreateMap<string, ICollection<CategoryDto>>().ConvertUsing(typeof(JsonDeserializeConverter<ICollection<CategoryDto>>));
            CreateMap<string, ICollection<Uri>>().ConvertUsing(typeof(JsonDeserializeConverter<ICollection<Uri>>));
            CreateMap<ICollection<PersonDto>, string>().ConvertUsing(typeof(JsonSerializeConverter<ICollection<PersonDto>>));
            CreateMap<ICollection<CategoryDto>, string>().ConvertUsing(typeof(JsonSerializeConverter<ICollection<CategoryDto>>));
            CreateMap<ICollection<Uri>, string>().ConvertUsing(typeof(JsonSerializeConverter<ICollection<Uri>>));

            //All for mapping from SyndicationFeed to to FeedDto to Feed entity.
            CreateMap<SyndicationPerson, PersonDto>();
            CreateMap<SyndicationCategory, CategoryDto>();
            CreateMap<SyndicationContent, string>().ConvertUsing(x => (x as TextSyndicationContent) != null ? ((TextSyndicationContent)x).Text : string.Empty);
            CreateMap<SyndicationLink, Uri>().ConvertUsing(x => x.Uri);
            CreateMap<SyndicationItem, FeedItemDto>()
                .ForMember(dest => dest.Links, opt => opt.MapFrom(x => x.Links.Select(y => y.Uri)))
                .ForMember(dest => dest.SourceFeed, opt => opt.Ignore());
            CreateMap<SyndicationFeed, FeedDto>()
                .ForMember(dest => dest.Documentation, opt => opt.MapFrom(x => x.Documentation.Uri));
            CreateMap<FeedItemDto, FeedItem>()//DTO will never overwrite these properties, only CreateDto can.
                .ForMember(dest => dest.IsFavorite, opt => opt.Ignore())
                .ForMember(dest => dest.IsRead, opt => opt.Ignore());
            CreateMap<FeedDto, Feed>();
            CreateMap<Feed, Feed>()//It's a new feed mapping to an old one.
                .ForMember(dest => dest.Pk, opt => opt.Ignore())
                .ForMember(dest => dest.SubscribeLink, opt => opt.Ignore())
                .ForMember(dest => dest.IsFavorite, opt => opt.Ignore())
                .ForMember(dest => dest.IsRead, opt => opt.Ignore())
                .ForMember(dest => dest.Items, opt => opt.Ignore());//dont map, we'll seperately handle it.

            CreateMap<FeedItem, FeedItemDto>();
            CreateMap<Feed, FeedDto>();
            CreateMap<FeedItemUpdateDto, FeedItem>();
            CreateMap<FeedCreateUpdateDto, Feed>();
        }
    }
    public class JsonDeserializeConverter<TDestination> : ITypeConverter<string, TDestination>
    {
        public TDestination Convert(string source, TDestination destination, ResolutionContext context)
        {
            if (source == null || source == string.Empty)
                return default(TDestination);
            return JsonSerializer.Deserialize<TDestination>(source);
        }
    }
    public class JsonSerializeConverter<TSource> : ITypeConverter<TSource, string>
    {
        public string Convert(TSource source, string destination, ResolutionContext context)
        {
            if (source == null)
                return string.Empty;
            return JsonSerializer.Serialize<TSource>(source);
        }
    }
}
