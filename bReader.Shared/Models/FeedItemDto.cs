namespace bReader.Shared.Models
{
    public class FeedItemDto
    {
        public int Pk { get; set; }
        public string Id { get; set; } = "";
        /// <summary>
        /// Get or set the value of whether the feed item has been read.
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// Get or set the value of whether the feed item has been marked as "Favorite".
        /// </summary>
        public bool IsFavorite { get; set; }
        public ICollection<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public string Content { get; set; } = "";
        public ICollection<PersonDto> Authors { get; set; } = new List<PersonDto>();
        public ICollection<Uri> Links { get; set; } = new List<Uri>();
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public FeedDto? SourceFeed { get; set; }
        public string Title { get; set; } = "";
        public string Summary { get; set; } = "";
    }
}
