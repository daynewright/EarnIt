using System.ComponentModel.DataAnnotations;

namespace EarnIt.ViewModels
{
    public class RewardCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImageURL { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int PointsNeeded { get; set; }
    }
}