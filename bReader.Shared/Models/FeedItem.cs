using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared.Models
{
    public class FeedItem
    {
        [Key]
        public int Pk { get; set; }//The SyndicationItem itself has a Id property, use Pk(for 'Primary Key') in case of confusion.
        public string Id { get; set; } = "";
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
        public string Categories { get; set; } = "[]";
        public string Content { get; set; } = "";
        /// <summary>
        /// Serilized json of <see cref="ICollection{PersonDto}"/>
        /// </summary>
        public string Authors { get; set; } = "[]";
        /// <summary>
        /// Serilized json of <see cref="ICollection{Uri}"/> 
        /// </summary>
        public string Links { get; set; } = "[]";
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        [Required]
        public Feed? SourceFeed { get; set; }
        public string Title { get; set; } = "";
        public string Summary { get; set; } = "";
    }



    /// <summary>
    /// Compares if the two FeedItem is the same, this might not be 100% accurate, using MD5 hash.
    /// </summary>
    public class FeedItemEqualityComparer : IEqualityComparer<FeedItem>
    {
        public bool Equals(FeedItem? x, FeedItem? y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;// if here, one is null while the other not.
            if (x.Id != y.Id) return false;
            //They should be same time, or same default
            if (x.LastUpdatedTime.DateTime != y.LastUpdatedTime.DateTime)
                return false;

            Func<FeedItem, byte[]> hash = (x) => HashAlgorithm.Create("MD5")?.ComputeHash(Encoding.UTF8.GetBytes(x.Id + x.Title + x.Summary + x.Content))
                                                    ??BitConverter.GetBytes(x.GetHashCode());//If MD5 is not usable on specific platform
            return hash(x) == hash(y);
        }


        public int GetHashCode([DisallowNull] FeedItem obj)
        {
            return new
            {
                Id = obj.Id,
                Title = obj.Title,
                LastUpdateTime = obj.LastUpdatedTime
            }.GetHashCode();
        }
    }
}
