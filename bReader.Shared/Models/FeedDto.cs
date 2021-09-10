using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared.Models
{
    public class FeedDto
    {
        public int Pk { get; set; }
        public string SubscribeLink { get; set; } = "";
        public bool IsRead { get; set; }
        public int UnreadCount { get; set; }
        public bool IsFavorite { get; set; }
        public FeedGroup Group { get; set; } = new FeedGroup();
        public string Title { get; set; } = "";
        public ICollection<PersonDto> Authors { get; set; } = new List<PersonDto>();
        public ICollection<FeedItemDto> Items { get; set; }=new List<FeedItemDto>();
        public DateTimeOffset LastUpdatedTime { get; set; }
        public string Language { get; set; } = "";
        public Uri? ImageUrl { get; set; }
        public string Copyright { get; set; } = "";
        public string Description { get; set; } = "";
        public ICollection<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public string Generator { get; set; } = "";
        public ICollection<Uri> Links { get; set; }=new List<Uri>();
        public Uri? Documentation { get; set; }
    }
}
