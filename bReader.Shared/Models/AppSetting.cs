using System.ComponentModel.DataAnnotations;

namespace bReader.Shared.Models
{
    public class AppSetting
    {
        public int Id { get; set; }
        [Required]
        public string? Key { get; set; }
        [Required]
        public string? Value { get; set; }
    }
}
