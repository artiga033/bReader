using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.ComponentModel.DataAnnotations;

namespace bReader.Shared.Models
{
    public class Feed
    {
        [Key]
        public int Pk { get; set; }//The rss doc itself has an "Id" property, in case of confusion
        [Url]
        public string SubscribeLink { get; set; }
        /// <summary>
        /// Get or set the value of whether the feed has been read.
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// Get or set the value of whether the feed has been marked as "Favorite".
        /// </summary>
        public bool IsFavorite { get; set; }

        public FeedGroup Group { get; set; }

        #region The SyndicationFeed Properties
        public string Title { get; set; }
        /// <summary>
        /// Serilized json of <see cref="ICollection{PersonDto}"/>
        /// </summary>
        public string Authors { get; set; }
        public ICollection<FeedItem> Items { get; set; } = new List<FeedItem>();
        public DateTimeOffset LastUpdatedTime { get; set; }
        public string Language { get; set; }
        public Uri ImageUrl { get; set; }
        public string Copyright { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Serilized json of <see cref="ICollection{CategoryDto}"/> 
        /// </summary>
        public string Categories { get; set; }
        public string Generator { get; set; }
        /// <summary>
        /// Serilized json of <see cref="ICollection{Uri}"/>
        /// </summary>
        public string Links { get; set; }
        public Uri Documentation { get; set; }
        #endregion
    }
}
