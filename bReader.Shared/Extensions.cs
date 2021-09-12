using bReader.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared
{
    public static class Extensions
    {
        public static List<FeedDto> GetAllFeeds(this IEnumerable<FeedGroupDto> groups)
        {
            List<FeedDto> r = new();
            foreach (var g in groups)
            {
                foreach (var f in g.Feeds)
                {
                    r.Add(f);
                }
            }
            return r;
        }
        public static List<Feed> GetAllFeeds(this IEnumerable<FeedGroup> groups)
        {
            List<Feed> r = new();
            foreach (var g in groups)
            {
                foreach (var f in g.Feeds)
                {
                    r.Add(f);
                }
            }
            return r;
        }
        public static void AddIfNone<Tkey,TValue> (this IDictionary<Tkey,TValue> dictionary,Tkey key,TValue value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key,value);
        }
        public static void AddIfNotNull<T>(this ICollection<T> collection,T? item)
        {
            if(item!=null)
                collection.Add(item);
        }
    }
}
