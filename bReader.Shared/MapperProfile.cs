using AutoMapper;
using bReader.Shared.Models;
using System.Text.Json;
using AutoMapper.Internal;
namespace bReader.Shared
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            this.Internal().ForAllPropertyMaps(p => p.DestinationMember == null, (p, cfg) => cfg.UseDestinationValue());
            CreateMap<string, Uri>().ConstructUsing(x => new Uri(x)).ReverseMap().ConvertUsing(x => x.ToString());
            CreateMap<string, ICollection<PersonDto>>().ConvertUsing(typeof(JsonDeserializeConverter<ICollection<PersonDto>>));
            CreateMap<string, ICollection<CategoryDto>>().ConvertUsing(typeof(JsonDeserializeConverter<ICollection<CategoryDto>>));
            CreateMap<string, ICollection<Uri>>().ConvertUsing(typeof(JsonDeserializeConverter<ICollection<Uri>>));
            CreateMap<ICollection<PersonDto>, string>().ConvertUsing(typeof(JsonSerializeConverter<ICollection<PersonDto>>));
            CreateMap<ICollection<CategoryDto>, string>().ConvertUsing(typeof(JsonSerializeConverter<ICollection<CategoryDto>>));
            CreateMap<ICollection<Uri>, string>().ConvertUsing(typeof(JsonSerializeConverter<ICollection<Uri>>));

            CreateMap<FeedItemDto, FeedItem>()//DTO will never overwrite these properties, only CreateDto can.
                .ForMember(dest => dest.IsFavorite, opt => opt.Ignore())
                .ForMember(dest => dest.IsRead, opt => opt.Ignore());
            CreateMap<FeedDto, Feed>();
            CreateMap<Feed, Feed>()//It's a new feed mapping to an old one.
                .ForMember(dest => dest.Pk, opt => opt.Ignore())
                .ForMember(dest => dest.SubscribeLink, opt => opt.Ignore())
                .ForMember(dest => dest.IsFavorite, opt => opt.Ignore())
                .ForMember(dest => dest.UnreadCount, opt => opt.Ignore())
                .ForMember(dest => dest.Group, opt => opt.Ignore())
                .ForMember(dest => dest.Items, opt => opt.Ignore());//dont map, we'll seperately handle it.

            CreateMap<FeedItem, FeedItemDto>();
            CreateMap<Feed, FeedDto>();
            CreateMap<FeedItemUpdateDto, FeedItem>();
            CreateMap<FeedCreateUpdateDto, Feed>();
            CreateMap<FeedGroup, FeedGroupDto>();
            CreateMap<FeedGroupCreateDto, FeedGroup>();
        }
    }
    public class JsonDeserializeConverter<TDestination> : ITypeConverter<string, TDestination>
    {
        public TDestination Convert(string source, TDestination destination, ResolutionContext context)
        {
            return JsonSerializer.Deserialize<TDestination>(source) ?? throw new ArgumentNullException(nameof(source));
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
