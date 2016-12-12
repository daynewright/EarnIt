using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarnIt.Models
{
    public class RewardEarned
    {
        [Key]
        public int RewardEarnedId { get; set; }

        public bool IsRedeemed { get; set; } = false;

        public DateTime? DateRedeemed { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateEarned { get; set; }
    }
}