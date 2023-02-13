using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class ExpertDTO
    {
        public int AccountId { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(25)]
        public string Password { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string FullName { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Company { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}
