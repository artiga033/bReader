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
        public string SubscribeLink { get; set; }
        public bool IsRead { get; set; }
        public bool IsFavorite { get; set; }
        public FeedGroup Group { get; set; }
        public string Title { get; set; }
        public ICollection<PersonDto> Authors { get; set; }
        public ICollection<FeedItemDto> Items { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public string Language { get; set; }
        public Uri ImageUrl { get; set; }
        public string Copyright { get; set; }
        public string Description { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }
        public string Generator { get; set; }
        public ICollection<Uri> Links { get; set; }
        public Uri Documentation { get; set; }
    }
}
