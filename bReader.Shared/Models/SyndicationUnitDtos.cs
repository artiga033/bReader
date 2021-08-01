using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace bReader.Shared.Models
{
    [Serializable]
    public class PersonDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
    }

    [Serializable]
    public class CategoryDto
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public string Scheme { get; set; }
    }

    //Why not directly use String
    //[Serializable]
    //public class TextContentDto
    //{
    //    public string Text { get; set; }
    //    public string Type { get; set; }
    //}
}
