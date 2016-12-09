using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarnIt.Models
{
    public class Reward
    {
        [Key]
        public int RewardId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [StringLength(55)]
        public string Description { get; set; }

        [StringLength(55)]
        public string ImageURL { get; set; }

        [Required]
        public int PointsNeeded { get; set; }

        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}