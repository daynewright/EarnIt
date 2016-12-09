using System.ComponentModel.DataAnnotations;

namespace EarnIt.ViewModels
{
    public class RewardViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageURL { get; set; }
        [Required]
        public int PointsNeeded { get; set; }
    }
}