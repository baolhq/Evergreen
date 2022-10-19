using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string FullName { get; set; }
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
        public string Role { get; set; } = "User";
        public bool IsBlocked { get; set; } = false;
        public string Professions { get; set; }
        public List<Message> Messages { get; set; }
    }
}
