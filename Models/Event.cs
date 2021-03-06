using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarnIt.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [DataTypeAttribute(DataType.Date)]
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
        [StringLength(25)]
        public string Type { get; set; }

        [Required]
        public bool AutoRefresh { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public int? Frequency { get; set; }

        [Required]
        public int ChildId { get; set; }
        public Child Child { get; set; }

        [ForeignKey("RewardId")]
        public int? RewardId { get; set; }
        public Reward Reward { get; set; }
    }
}