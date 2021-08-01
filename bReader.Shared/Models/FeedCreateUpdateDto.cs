using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared.Models
{
    public class FeedCreateUpdateDto
    {
        [Key][Url]
        public string SubscribeLink { get; set; }
        // User won't set this, when all items' IsRead were true, this would be true.
        //public bool IsRead { get; set; }
        /// <summary>
        /// Get or set the value of whether the feed has been marked as "Favorite".
        /// </summary>
        public bool IsFavorite { get; set; }
        public FeedGroupDto FeedGroup { get; set; }
    }
}
