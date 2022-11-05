using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
