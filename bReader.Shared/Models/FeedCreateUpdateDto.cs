﻿using System.ComponentModel.DataAnnotations;

namespace bReader.Shared.Models
{
    public class FeedCreateUpdateDto
    {
        [Key]
        [Url]
        [Required]
        public string? SubscribeLink { get; set; }
        // User won't set this, when all items' IsRead were true, this would be true.
        //public bool IsRead { get; set; }
        /// <summary>
        /// Get or set the value of whether the feed has been marked as "Favorite".
        /// </summary>
        public bool IsFavorite { get; set; }
        [Required]
        public string? FeedGroupName { get; set; }
    }
}
