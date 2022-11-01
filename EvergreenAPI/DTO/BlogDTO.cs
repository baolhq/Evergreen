﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.DTO
{
    public class BlogDTO
    {
        public int BlogId { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(500)]
        public string Content { get; set; }
        [Required]
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
        [Required]
        public int ViewCount { get; set; } = 0;
    }
}
