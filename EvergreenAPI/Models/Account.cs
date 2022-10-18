﻿using System.Collections.Generic;
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
        public bool IsAdmin { get; set; } = false;
        public bool IsProfessor { get; set; } = false;
        public List<string> Professions { get; set; } = new List<string>();
        public bool IsBlocked { get; set; } = false;
    }
}
