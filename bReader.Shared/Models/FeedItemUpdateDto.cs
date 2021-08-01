using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared.Models
{
    class FeedItemUpdateDto
    {
        public string Id { get; set; }
        /// <summary>
        /// Get or set the value of whether the feed item has been read.
        /// </summary>
        public bool IsRead { get; set; } 
        /// <summary>
        /// Get or set the value of whether the feed item has been marked as "Favorite".
        /// </summary>
        public bool IsFavorite { get; set; } 
    }
}
