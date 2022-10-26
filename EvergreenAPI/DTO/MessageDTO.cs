using EvergreenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.DTO
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        public int ReplyToId { get; set; } = 0;
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public int AccountId { get; set; }
        public Account Author { get; set; }
    }
}
