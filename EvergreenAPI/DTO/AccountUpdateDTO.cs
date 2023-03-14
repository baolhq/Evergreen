using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class AccountUpdateDTO
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter correct phone num")]
        [StringLength(11, MinimumLength = 10)]
        public string PhoneNumber { get; set; }
        public string Professions { get; set; }
        public string Bio { get; set; }
        public string AvatarUrl { get; set; } = "https://i.imgur.com/n1rrde0.png";
    }
}
