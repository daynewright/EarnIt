using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EarnIt.Models;

namespace user_auth.Models
{
    public class Child 
    {
        [Key]
        public int ChildId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set ; }

        [StringLength(55)]
        public string ImageURL { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}