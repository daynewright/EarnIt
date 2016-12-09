using System.ComponentModel.DataAnnotations;
using EarnIt.Data;
using EarnIt.Models;

namespace EarnIt.ViewModels
{
    public class EventCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImageURL { get; set; }

        [Required]
        public string Type { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public int ChildId { get; set; }

        public int? Frequency { get; set; }

        [Required]
        public bool AutoRefresh {get; set; }

        //public EventCreateViewModel(ApplicationDbContext ctx, ApplicationUser user) : base(ctx, user) {}
    }
}