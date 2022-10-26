using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.DTO
{
    public class BlogDTO
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int ViewCount { get; set; } = 0;
    }
}
