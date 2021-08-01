using bReader.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared
{
    public static class Extensions
    {
        public static FeedItemDto ToFeedItemDto(this SyndicationItem synFeedItem)
        {
            FeedItemDto result = new FeedItemDto();
            throw new NotImplementedException();
        }
        public static FeedDto ToFeedDto(this SyndicationFeed synFeed)
        {
            throw new NotImplementedException();
        } 
    }
}
