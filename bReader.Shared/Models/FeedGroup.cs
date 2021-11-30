using System.ComponentModel.DataAnnotations;

namespace bReader.Shared.Models
{
    public class FeedGroup
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public ICollection<Feed> Feeds { get; set; } = new List<Feed>();
    }
    public class FeedGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ICollection<FeedDto> Feeds { get; set; } = new List<FeedDto>();
    }
    public class FeedGroupCreateDto
    {
        [Required]
        public string? Name { get; set; }
    }
}
