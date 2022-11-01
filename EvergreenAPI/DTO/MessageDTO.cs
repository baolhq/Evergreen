using EvergreenAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.DTO
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        [Required]
        public int ReplyToId { get; set; } = 0;
        [Required]
        [MaxLength(100)]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [Required]
        public int AccountId { get; set; }
        public AccountDTO Author { get; set; }
    }
}
