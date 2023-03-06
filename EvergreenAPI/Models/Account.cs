using System;
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
        [Required(ErrorMessage = "Cannot be blank")]
        [Column(TypeName = "nvarchar(255)")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Cannot be blank")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,32}$", ErrorMessage = "Use more than 7 characters include letters and numbers")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Cannot be blank"), Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Cannot be blank")]
        [Column(TypeName = "nvarchar(255)")]
        public string FullName { get; set; }


        [MaxLength(255)]
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email, Please Re-Enter")]
        public string Email { get; set; } = string.Empty;

        public bool Status { get; set; } = true;

        public string Role { get; set; }
        [Required]
        public string Token { get; set; }
        public bool IsBlocked { get; set; } = false;

        [Required(ErrorMessage = "Cannot be blank")]
        public string Professions { get; set; }



        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Phone Number")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter correct phone num")]
        [StringLength(11, MinimumLength = 10)]
        public string PhoneNumber { get; set; }



        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public DateTime? VerifiedAt { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }

    }

        
    
}
