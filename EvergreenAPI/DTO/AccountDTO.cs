using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace EvergreenAPI.DTO
{
    public class AccountDTO
    {
        public int AccountId { get; set; }


        [Column(TypeName = "nvarchar(255)")] public string Username { get; set; }

        [Column(TypeName = "nvarchar(255)")] public string FullName { get; set; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Invalid email, Please Re-Enter")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cannot be blank")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,32}$",
            ErrorMessage = "Use more than 7 characters include letters and numbers")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [NotMapped]
        [Required(ErrorMessage = "Cannot be blank"),
         Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string Professions { get; set; }
        public DateTime? VerifiedAt { get; set; }


        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Phone Number")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter correct phone num")]
        [StringLength(11, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        public bool IsBlocked { get; set; } = false;
        public string AvatarUrl { get; set; } = "https://i.imgur.com/n1rrde0.png";

        public IFormFile AvatarImg { get; set; }
        public string Bio { get; set; }
    }
}