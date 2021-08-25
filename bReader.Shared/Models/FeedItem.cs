using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared.Models
{
    public class FeedItem
    {
        [Key]
        public int Pk { get; set; }//The SyndicationItem itself has a Id property, use Pk(for 'Primary Key') in case of confusion.
        public string Id { get; set; }
        /// <summary>
        /// Get or set the value of whether the feed item has been read.
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// Get or set the value of whether the feed item has been marked as "Favorite".
        /// </summary>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// Serilized json of <see cref="ICollection{CategoryDto}"/> 
        /// </summary>
        public string Categories { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// Serilized json of <see cref="ICollection{PersonDto}"/>
        /// </summary>
        public string Authors { get; set; }
        /// <summary>
        /// Serilized json of <see cref="ICollection{Uri}"/> 
        /// </summary>
        public string Links { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public Feed SourceFeed { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
    }


    // TODO: A better comparer is required, for most sites/generators don't exactly fill all the fields like pubDate and UpdateTime
    /// <summary>
    /// Compares if the two FeedItem is same 'old', that is , they has sameId and same LastUpdatedTime(if any)
    /// </summary>
    public class FeedItemSameOldEqualityComparer : IEqualityComparer<FeedItem>
    {
        public bool Equals(FeedItem x, FeedItem y)
        {
            if (x.Id == null || y.Id == null)
                return false;
            if (x.Id != y.Id) return false;
            var dValue = default(DateTime);
            //if one of them is default, then we'd better update it
            if (x.LastUpdatedTime.DateTime == dValue || y.LastUpdatedTime.DateTime == dValue)
                return false;
            //not default but the same, then they are the same!
            if (x.LastUpdatedTime.DateTime == y.LastUpdatedTime.DateTime)
                return true;
            else
                return false;
        }


        public int GetHashCode([DisallowNull] FeedItem obj)
        {
            return new
            {
                Id = obj.Id,
                LastUpdateTime = obj.LastUpdatedTime
            }.GetHashCode();
        }
    }
}
