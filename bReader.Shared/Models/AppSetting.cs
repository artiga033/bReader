using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared.Models
{
    public class AppSetting
    {
        public int  Id { get; set; }
        [Required]
        public string? Key { get; set; }
        [Required]
        public string? Value { get; set; }
    }
}
