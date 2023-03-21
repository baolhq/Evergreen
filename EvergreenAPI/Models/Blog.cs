using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime LastModifiedDate { get; set; }
        public int ViewCount { get; set; } = 0;

        [ForeignKey("Thumbnails")]
        [DisplayName("Image Description")]
        public int ThumbnailId { get; set; }
        public virtual Thumbnail Thumbnail { get; set; }
    }
}
