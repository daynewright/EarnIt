using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarnIt.Models
{
    public class EventPoint
    {
        [Required]
        public int EventPointId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        [Required]
        public int Point { get; set; }

        [Required]
        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}