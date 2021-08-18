using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared.Models
{
    public class FeedGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Feed> Feeds { get; set; } = new List<Feed>();
    }
    public class FeedGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<FeedDto> Feeds { get; set; }
    }
    public class FeedGroupCreateDto
    {
        public string Name { get; set; }
    }
}
