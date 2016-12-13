using System.ComponentModel.DataAnnotations;

namespace EarnIt.Models.AccountViewModels
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Id { get; set; }

    }
}