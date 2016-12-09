using System.ComponentModel.DataAnnotations;
using EarnIt.Data;
using EarnIt.Models;

namespace EarnIt.ViewModels
{
    public class ChildCreateViewModel
    {
        [Required]
        [StringLength(25)]
        public string Name { get; set ; }

        [StringLength(55)]
        public string ImageURL { get; set; }

        [Required]
        public int Age { get; set; }
    }
}