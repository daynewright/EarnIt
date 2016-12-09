using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarnIt.Models
{
    public class RewardEarned
    {
        [Required]
        public int RewardEarnedId { get; set; }

        [Required]
        public bool Redeemed {get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        [Required]
        public int EventPointId { get; set; }
        public EventPoint EventPoint { get; set; }
    }
}