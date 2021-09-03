using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared.Utils
{
    public static class SettingKeyMap
    {
        [DefaultValue("60")]
        public const string SourceUpdatePeriod = "SourceUpdatePeriod";
        [DefaultValue("15")]
        public const string PageSize = "PageSize";
        
    }
    [AttributeUsage(AttributeTargets.Field,AllowMultiple =false,Inherited =false)]
    public class DefaultValueAttribute : Attribute
    {
        public string Value { get; set; }
        public DefaultValueAttribute(string value)
        {
            this.Value = value;
        }
    }
}
