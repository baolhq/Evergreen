using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.DTO
{
    public class UserDTO
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
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}
