using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        public int ReplyToId { get; set; } = 0; // 0 = not a reply
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [ForeignKey("Accounts")]
        public int AccountId { get; set; }
        public virtual Account Author { get; set; }
    }
}
