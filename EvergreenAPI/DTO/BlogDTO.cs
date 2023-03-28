using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.DTO
{
    public class BlogDTO
    {
        public int BlogId { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        [Required]
       
        public string Content { get; set; }
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
        public int ViewCount { get; set; } = 0;
        [Required]
        public int ThumbnailId { get; set; }
    }
}
