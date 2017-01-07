using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EarnIt.Models;

namespace EarnIt.ViewModels
{ 
    public class EventViewModel
    {
        [Required]
        public int EventId { get; set; }
        public int? RewardId { get; set; }
        public Reward Reward { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Type {get; set; }
        [Required]
        public string ImageURL { get; set; }
        [Required]
        public bool AutoRefresh { get; set; } = true;
        [Required]
        public bool IsActive { get; set; } = true;
        [Required]
        public int? Frequency { get; set; }
    }
}