using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class AccountDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
