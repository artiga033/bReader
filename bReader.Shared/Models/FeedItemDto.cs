using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared.Models
{
    public class FeedItemDto
    {
        public int Pk { get; set; }
        public string Id { get; set; }
        /// <summary>
        /// Get or set the value of whether the feed item has been read.
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// Get or set the value of whether the feed item has been marked as "Favorite".
        /// </summary>
        public bool IsFavorite { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }
        public string Content { get; set; }
        public ICollection<PersonDto> Authors { get; set; }
        public ICollection<Uri> Links { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public Feed SourceFeed { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
    }
}
